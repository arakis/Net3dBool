/*
The MIT License (MIT)

Copyright (c) 2014 Sebastian Loncar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

See:
D. H. Laidlaw, W. B. Trumbore, and J. F. Hughes.
"Constructive Solid Geometry for Polyhedral Objects"
SIGGRAPH Proceedings, 1986, p.161.

original author: Danilo Balby Silva Castanheira (danbalby@yahoo.com)

Ported from Java to C# by Sebastian Loncar, Web: http://www.loncar.de
Project: https://github.com/Arakis/Net3dBool

Optimized and refactored by: Lars Brubaker (larsbrubaker@matterhackers.com)
Project: https://github.com/MatterHackers/agg-sharp (an included library)
*/

using System;

namespace Net3dBool
{
    public enum Status { UNKNOWN, INSIDE, OUTSIDE, SAME, OPPOSITE, BOUNDARY };

    /// <summary>
    /// Representation of a 3D face (triangle).
    /// </summary>
    public class Face //: IPrimitive
    {
        /** first vertex */
        public Vertex V1;
        /** second vertex */
        public Vertex V2;
        /** third vertex */
        public Vertex V3;

        private Vector3 Center;

        /** face status relative to a solid  */
        private readonly static double EqualityTolerance = 1e-10f;
        private enum Side { UP, DOWN, ON, NONE };
        private Bound BoundCache;
        private bool CachedBounds = false;
        private Plane PlaneCache;
        private Status Status;

        /** face status if it is still unknown */
        /** face status if it is inside a solid */
        /** face status if it is outside a solid */
        /** face status if it is coincident with a solid face */
        /** face status if it is coincident with a solid face with opposite orientation*/
        /** point status if it is up relative to an edge - see linePositionIn_ methods */
        /** point status if it is down relative to an edge - see linePositionIn_ methods */
        /** point status if it is on an edge - see linePositionIn_ methods */
        /** point status if it isn't up, down or on relative to an edge - see linePositionIn_ methods */
        /** tolerance value to test equalities */
        //---------------------------------CONSTRUCTORS---------------------------------//

        /// <summary>
        /// Default constructor
        /// </summary>
        private Face()
        {
        }

        /// <summary>
        /// * Constructs a face with unknown status.
        /// </summary>
        /// <param name="v1">a face vertex</param>
        /// <param name="v2">a face vertex</param>
        /// <param name="v3">a face vertex</param>
        public Face(Vertex v1, Vertex v2, Vertex v3)
        {
            this.V1 = v1;
            this.V2 = v2;
            this.V3 = v3;
            Center = (v1._Position + v2._Position + v3._Position) / 3.0;

            Status = Status.UNKNOWN;
        }

        /// <summary>
        /// Clones the face object
        /// </summary>
        /// <returns>cloned face object</returns>
        public Face Clone()
        {
            Face clone = new Face();
            clone.V1 = V1.Clone();
            clone.V2 = V2.Clone();
            clone.V3 = V3.Clone();
            clone.Center = Center;
            clone.Status = Status;
            return clone;
        }

        /**
        * Makes a string definition for the Face object
        *
        * @return the string definition
        */

        public bool Equals(Face face)
        {
            bool cond1 = V1.Equals(face.V1) && V2.Equals(face.V2) && V3.Equals(face.V3);
            bool cond2 = V1.Equals(face.V2) && V2.Equals(face.V3) && V3.Equals(face.V1);
            bool cond3 = V1.Equals(face.V3) && V2.Equals(face.V1) && V3.Equals(face.V2);

            return cond1 || cond2 || cond3;
        }

        public double GetIntersectCost()
        {
            return 350;
        }

        public Vector3 GetCenter()
        {
            return Center;
        }

        public double GetArea()
        {
            //area = (a * c * sen(B))/2
            Vector3 p1 = V1.Position;
            Vector3 p2 = V2.Position;
            Vector3 p3 = V3.Position;
            Vector3 xy = new Vector3(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            Vector3 xz = new Vector3(p3.X - p1.X, p3.Y - p1.Y, p3.Z - p1.Z);

            double a = (p1 - p2).Length;
            double c = (p1 - p3).Length;
            double B = Vector3.CalculateAngle(xy, xz);

            return (a * c * Math.Sin(B)) / 2d;
        }

        public Bound GetBound()
        {
            if (!CachedBounds)
            {
                BoundCache = new Bound(V1.Position, V2.Position, V3.Position);
                CachedBounds = true;
            }

            return BoundCache;
        }

        public Plane GetPlane()
        {
            if (PlaneCache == null)
            {
                Vector3 p1 = V1.Position;
                Vector3 p2 = V2.Position;
                Vector3 p3 = V3.Position;
                PlaneCache = new Plane(p1, p2, p3);
            }

            return PlaneCache;
        }

        public Vector3 GetNormal()
        {
            return GetPlane().PlaneNormal;
        }

        public Status GetStatus()
        {
            return Status;
        }

        public void Invert()
        {
            Vertex vertexTemp = V2;
            V2 = V1;
            V1 = vertexTemp;
        }

        /// <summary>
        /// Classifies the face based on the ray trace technique
        /// </summary>
        /// <param name="obj">object3d used to compute the face status</param>
        public void RayTraceClassify(Object3D obj)
        {
            //creating a ray starting at the face baricenter going to the normal direction
            Line ray = new Line(GetNormal(), Center);

            bool success;
            double distance;
            Vector3 intersectionPoint;
            Face closestFace = null;
            double closestDistance;

            do
            {
                success = true;
                closestDistance = Double.MaxValue;
                //for each face from the other solid...
                for (int faceIndex = 0; faceIndex < obj.GetNumFaces(); faceIndex++)
                {
                    Face face = obj.GetFace(faceIndex);
                    intersectionPoint = ray.ComputePlaneIntersection(face.GetPlane());

                    //if ray intersects the plane...
                    if (intersectionPoint.X != double.PositiveInfinity)
                    {
                        double dotProduct = Vector3.Dot(face.GetNormal(), ray.Direction);
                        distance = ray.ComputePointToPointDistance(intersectionPoint);

                        //if ray lies in plane...
                        if (Math.Abs(distance) < EqualityTolerance && Math.Abs(dotProduct) < EqualityTolerance)
                        {
                            //disturb the ray in order to not lie into another plane
                            ray.PerturbDirection();
                            success = false;
                            break;
                        }

                        //if ray starts in plane...
                        if (Math.Abs(distance) < EqualityTolerance && Math.Abs(dotProduct) > EqualityTolerance)
                        {
                            //if ray intersects the face...
                            if (face.ContainsPoint(intersectionPoint))
                            {
                                //faces coincide
                                closestFace = face;
                                closestDistance = 0;
                                break;
                            }
                        }

                        //if ray intersects plane...
                        else if (Math.Abs(dotProduct) > EqualityTolerance && distance > EqualityTolerance)
                        {
                            if (distance < closestDistance)
                            {
                                //if ray intersects the face;
                                if (face.ContainsPoint(intersectionPoint))
                                {
                                    //this face is the closest face untill now
                                    closestDistance = distance;
                                    closestFace = face;
                                }
                            }
                        }
                    }
                }
            } while (success == false);


            if (closestFace == null)
            {
                //none face found: outside face
                Status = Status.OUTSIDE;
            }
            else //face found: test dot product
            {
                double dotProduct = Vector3.Dot(closestFace.GetNormal(), ray.Direction);

                //distance = 0: coplanar faces
                if (Math.Abs(closestDistance) < EqualityTolerance)
                {
                    if (dotProduct > EqualityTolerance)
                    {
                        Status = Status.SAME;
                    }
                    else if (dotProduct < -EqualityTolerance)
                    {
                        Status = Status.OPPOSITE;
                    }
                }
                else if (dotProduct > EqualityTolerance)
                {
                    //dot product > 0 (same direction): inside face
                    Status = Status.INSIDE;
                }
                else if (dotProduct < -EqualityTolerance)
                {
                    //dot product < 0 (opposite direction): outside face
                    Status = Status.OUTSIDE;
                }
            }
        }

        /// <summary>
        /// Classifies the face if one of its vertices are classified as INSIDE or OUTSIDE
        /// </summary>
        /// <returns>true if the face could be classified, false otherwise</returns>
        public bool SimpleClassify()
        {
            Status status1 = V1.Status;
            Status status2 = V2.Status;
            Status status3 = V3.Status;

            if (status1 == Status.INSIDE || status1 == Status.OUTSIDE)
            {
                this.Status = status1;
                return true;
            }
            else if (status2 == Status.INSIDE || status2 == Status.OUTSIDE)
            {
                this.Status = status2;
                return true;
            }
            else if (status3 == Status.INSIDE || status3 == Status.OUTSIDE)
            {
                this.Status = status3;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return V1.ToString() + "\n" + V2.ToString() + "\n" + V3.ToString();
        }

        /**
        * Checks if a face is equal to another. To be equal, they have to have equal
        * vertices in the same order
        *
        * @param anObject the other face to be tested
        * @return true if they are equal, false otherwise.
        */
        //-------------------------------------GETS-------------------------------------//

        /**
        * Gets the face bound
        *
        * @return face bound
        */
        /**
        * Gets the face normal
        *
        * @return face normal
        */
        /**
        * Gets the face status
        *
        * @return face status - UNKNOWN, INSIDE, OUTSIDE, SAME OR OPPOSITE
        */
        /**
        * Gets the face area
        *
        * @return face area
        */

        //-------------------------------------OTHERS-----------------------------------//

        /** Invert face direction (normal direction) */

        //------------------------------------PRIVATES----------------------------------//

        /// <summary>
        /// Gets the position of a point relative to a line in the x plane
        /// </summary>
        /// <param name="point">point to be tested</param>
        /// <param name="pointLine1">one of the line ends</param>
        /// <param name="pointLine2">one of the line ends</param>
        /// <returns>position of the point relative to the line - UP, DOWN, ON, NONE</returns>
        private static Side LinePositionInX(Vector3 point, Vector3 pointLine1, Vector3 pointLine2)
        {
            double a, b, z;
            if ((Math.Abs(pointLine1.Y - pointLine2.Y) > EqualityTolerance) && (((point.Y >= pointLine1.Y) && (point.Y <= pointLine2.Y)) || ((point.Y <= pointLine1.Y) && (point.Y >= pointLine2.Y))))
            {
                a = (pointLine2.Z - pointLine1.Z) / (pointLine2.Y - pointLine1.Y);
                b = pointLine1.Z - a * pointLine1.Y;
                z = a * point.Y + b;
                if (z > point.Z + EqualityTolerance)
                {
                    return Side.UP;
                }
                else if (z < point.Z - EqualityTolerance)
                {
                    return Side.DOWN;
                }
                else
                {
                    return Side.ON;
                }
            }
            else
            {
                return Side.NONE;
            }
        }

        /// <summary>
        /// Gets the position of a point relative to a line in the y plane
        /// </summary>
        /// <param name="point">point to be tested</param>
        /// <param name="pointLine1">one of the line ends</param>
        /// <param name="pointLine2">one of the line ends</param>
        /// <returns>position of the point relative to the line - UP, DOWN, ON, NONE</returns>
        private static Side LinePositionInY(Vector3 point, Vector3 pointLine1, Vector3 pointLine2)
        {
            double a, b, z;
            if ((Math.Abs(pointLine1.X - pointLine2.X) > EqualityTolerance) && (((point.X >= pointLine1.X) && (point.X <= pointLine2.X)) || ((point.X <= pointLine1.X) && (point.X >= pointLine2.X))))
            {
                a = (pointLine2.Z - pointLine1.Z) / (pointLine2.X - pointLine1.X);
                b = pointLine1.Z - a * pointLine1.X;
                z = a * point.X + b;
                if (z > point.Z + EqualityTolerance)
                {
                    return Side.UP;
                }
                else if (z < point.Z - EqualityTolerance)
                {
                    return Side.DOWN;
                }
                else
                {
                    return Side.ON;
                }
            }
            else
            {
                return Side.NONE;
            }
        }

        /// <summary>
        /// Gets the position of a point relative to a line in the z plane
        /// </summary>
        /// <param name="point">point to be tested</param>
        /// <param name="pointLine1">one of the line ends</param>
        /// <param name="pointLine2">one of the line ends</param>
        /// <returns>position of the point relative to the line - UP, DOWN, ON, NONE</returns>
        private static Side LinePositionInZ(Vector3 point, Vector3 pointLine1, Vector3 pointLine2)
        {
            double a, b, y;
            if ((Math.Abs(pointLine1.X - pointLine2.X) > EqualityTolerance) && (((point.X >= pointLine1.X) && (point.X <= pointLine2.X)) || ((point.X <= pointLine1.X) && (point.X >= pointLine2.X))))
            {
                a = (pointLine2.Y - pointLine1.Y) / (pointLine2.X - pointLine1.X);
                b = pointLine1.Y - a * pointLine1.X;
                y = a * point.X + b;
                if (y > point.Y + EqualityTolerance)
                {
                    return Side.UP;
                }
                else if (y < point.Y - EqualityTolerance)
                {
                    return Side.DOWN;
                }
                else
                {
                    return Side.ON;
                }
            }
            else
            {
                return Side.NONE;
            }
        }

        /// <summary>
        /// Checks if the the face contains a point
        /// </summary>
        /// <param name="point">point to be tested</param>
        /// <returns>true if the face contains the point, false otherwise</returns>
        private bool ContainsPoint(Vector3 point)
        {
            Side result1;
            Side result2;
            Side result3;
            Vector3 normal = GetNormal();

            //if x is constant...
            if (Math.Abs(normal.X) > EqualityTolerance)
            {
                //tests on the x plane
                result1 = LinePositionInX(point, V1.Position, V2.Position);
                result2 = LinePositionInX(point, V2.Position, V3.Position);
                result3 = LinePositionInX(point, V3.Position, V1.Position);
            }

            //if y is constant...
            else if (Math.Abs(normal.Y) > EqualityTolerance)
            {
                //tests on the y plane
                result1 = LinePositionInY(point, V1.Position, V2.Position);
                result2 = LinePositionInY(point, V2.Position, V3.Position);
                result3 = LinePositionInY(point, V3.Position, V1.Position);
            }
            else
            {
                //tests on the z plane
                result1 = LinePositionInZ(point, V1.Position, V2.Position);
                result2 = LinePositionInZ(point, V2.Position, V3.Position);
                result3 = LinePositionInZ(point, V3.Position, V1.Position);
            }

            //if the point is up and down two lines...
            if (((result1 == Side.UP) || (result2 == Side.UP) || (result3 == Side.UP)) && ((result1 == Side.DOWN) || (result2 == Side.DOWN) || (result3 == Side.DOWN)))
            {
                return true;
            }
            //if the point is on of the lines...
            else if ((result1 == Side.ON) || (result2 == Side.ON) || (result3 == Side.ON))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}