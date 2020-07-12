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
using OpenToolkit.Mathematics;

namespace Net3dBool
{
    /// <summary>
    /// Representation of a 3d line or a ray(represented by a direction and a point).
    /// </summary>
    public class Line
    {
        /// <summary>
        /// tolerance value to test equalities
        /// </summary>
        private readonly static double EqualityTolerance = 1e-10f;
        private static Random Rnd = new Random();
        private Vector3d StartPoint;

        /// <summary>
        /// Constructor for a line.The line created is the intersection between two planes
        /// </summary>
        /// <param name="face1">face representing one of the planes</param>
        /// <param name="face2">face representing one of the planes</param>
        public Line(Face face1, Face face2)
        {
            Vector3d normalFace1 = face1.GetNormal();
            Vector3d normalFace2 = face2.GetNormal();

            //direction: cross product of the faces normals
            Direction = Vector3d.Cross(normalFace1, normalFace2);

            //if direction lenght is not zero (the planes aren't parallel )...
            if (!(Direction.Length < EqualityTolerance))
            {
                //getting a line point, zero is set to a coordinate whose direction
                //component isn't zero (line intersecting its origin plan)
                StartPoint = new Vector3d();
                double d1 = -(normalFace1.X * face1.V1._Position.X + normalFace1.Y * face1.V1._Position.Y + normalFace1.Z * face1.V1._Position.Z);
                double d2 = -(normalFace2.X * face2.V1._Position.X + normalFace2.Y * face2.V1._Position.Y + normalFace2.Z * face2.V1._Position.Z);
                if (Math.Abs(Direction.X) > EqualityTolerance)
                {
                    StartPoint.X = 0;
                    StartPoint.Y = (d2 * normalFace1.Z - d1 * normalFace2.Z) / Direction.X;
                    StartPoint.Z = (d1 * normalFace2.Y - d2 * normalFace1.Y) / Direction.X;
                }
                else if (Math.Abs(Direction.Y) > EqualityTolerance)
                {
                    StartPoint.X = (d1 * normalFace2.Z - d2 * normalFace1.Z) / Direction.Y;
                    StartPoint.Y = 0;
                    StartPoint.Z = (d2 * normalFace1.X - d1 * normalFace2.X) / Direction.Y;
                }
                else
                {
                    StartPoint.X = (d2 * normalFace1.Y - d1 * normalFace2.Y) / Direction.Z;
                    StartPoint.Y = (d1 * normalFace2.X - d2 * normalFace1.X) / Direction.Z;
                    StartPoint.Z = 0;
                }
            }

            Direction.Normalize();
        }

        /// <summary>
        /// Constructor for a ray
        /// </summary>
        /// <param name="direction">direction ray</param>
        /// <param name="point">beginning of the ray</param>
        public Line(Vector3d direction, Vector3d point)
        {
            Direction = direction;
            StartPoint = point;
            direction.Normalize();
        }

        private Line()
        {
        }

        /// <summary>
        /// line direction
        /// </summary>
        public Vector3d Direction { get; private set; }

        public Line Clone()
        {
            Line clone = new Line();
            clone.Direction = Direction;
            clone.StartPoint = StartPoint;
            return clone;
        }

        /// <summary>
        /// Computes the point resulting from the intersection with another line
        /// </summary>
        /// <param name="otherLine">the other line to apply the intersection. The lines are supposed to intersect</param>
        /// <returns>point resulting from the intersection. If the point coundn't be obtained, return null</returns>
        public Vector3d ComputeLineIntersection(Line otherLine)
        {
            //x = x1 + a1*t = x2 + b1*s
            //y = y1 + a2*t = y2 + b2*s
            //z = z1 + a3*t = z2 + b3*s

            Vector3d linePoint = otherLine.GetPoint();
            Vector3d lineDirection = otherLine.Direction;

            double t;
            if (Math.Abs(Direction.Y * lineDirection.X - Direction.X * lineDirection.Y) > EqualityTolerance)
            {
                t = (-StartPoint.Y * lineDirection.X + linePoint.Y * lineDirection.X + lineDirection.Y * StartPoint.X - lineDirection.Y * linePoint.X) / (Direction.Y * lineDirection.X - Direction.X * lineDirection.Y);
            }
            else if (Math.Abs(-Direction.X * lineDirection.Z + Direction.Z * lineDirection.X) > EqualityTolerance)
            {
                t = -(-lineDirection.Z * StartPoint.X + lineDirection.Z * linePoint.X + lineDirection.X * StartPoint.Z - lineDirection.X * linePoint.Z) / (-Direction.X * lineDirection.Z + Direction.Z * lineDirection.X);
            }
            else if (Math.Abs(-Direction.Z * lineDirection.Y + Direction.Y * lineDirection.Z) > EqualityTolerance)
            {
                t = (StartPoint.Z * lineDirection.Y - linePoint.Z * lineDirection.Y - lineDirection.Z * StartPoint.Y + lineDirection.Z * linePoint.Y) / (-Direction.Z * lineDirection.Y + Direction.Y * lineDirection.Z);
            }
            else
            {
#if DEBUG
                throw new InvalidOperationException();
#else
				return Vector3.Zero;
#endif
            }

            double x = StartPoint.X + Direction.X * t;
            double y = StartPoint.Y + Direction.Y * t;
            double z = StartPoint.Z + Direction.Z * t;

            return new Vector3d(x, y, z);
        }

        /// <summary>
        /// Compute the point resulting from the intersection with a plane
        /// </summary>
        /// <param name="normal">the plane normal</param>
        /// <param name="planePoint">a plane point.</param>
        /// <returns>intersection point.If they don't intersect, return null</returns>
        public Vector3d ComputePlaneIntersection(Plane plane)
        {
            double distanceToStartFromOrigin = Vector3d.Dot(plane.PlaneNormal, StartPoint);

            double distanceFromPlane = distanceToStartFromOrigin - plane.DistanceToPlaneFromOrigin;
            double denominator = Vector3d.Dot(plane.PlaneNormal, Direction);

            if (Math.Abs(denominator) < EqualityTolerance)
            {
                //if line is paralel to the plane...
                if (Math.Abs(distanceFromPlane) < EqualityTolerance)
                {
                    //if line is contained in the plane...
                    return StartPoint;
                }
                else
                {
                    return Vector3d.PositiveInfinity;
                }
            }
            else // line intercepts the plane...
            {
                double t = -distanceFromPlane / denominator;
                Vector3d resultPoint = new Vector3d();
                resultPoint.X = StartPoint.X + t * Direction.X;
                resultPoint.Y = StartPoint.Y + t * Direction.Y;
                resultPoint.Z = StartPoint.Z + t * Direction.Z;

                return resultPoint;
            }
        }

        /// <summary>
        /// Computes the distance from the line point to another point
        /// </summary>
        /// <param name="otherPoint">the point to compute the distance from the line point. The point is supposed to be on the same line.</param>
        /// <returns>points distance. If the point submitted is behind the direction, the distance is negative</returns>
        public double ComputePointToPointDistance(Vector3d otherPoint)
        {
            double distance = (otherPoint - StartPoint).Length;
            Vector3d vec = new Vector3d(otherPoint.X - StartPoint.X, otherPoint.Y - StartPoint.Y, otherPoint.Z - StartPoint.Z);
            vec.Normalize();
            if (Vector3d.Dot(vec, Direction) < 0)
            {
                return -distance;
            }
            else
            {
                return distance;
            }
        }

        public Vector3d GetPoint()
        {
            return StartPoint;
        }

        /// <summary>
        /// Changes slightly the line direction
        /// </summary>
        public void PerturbDirection()
        {
            Vector3d perturbedDirection = Direction;
            perturbedDirection.X += 1e-5 * Random();
            perturbedDirection.Y += 1e-5 * Random();
            perturbedDirection.Z += 1e-5 * Random();

            Direction = perturbedDirection;
        }

        public void SetDirection(Vector3d direction)
        {
            Direction = direction;
        }

        public void SetPoint(Vector3d point)
        {
            StartPoint = point;
        }

        public string toString()
        {
            return "Direction: " + Direction.ToString() + "\nPoint: " + StartPoint.ToString();
        }

        private static double Random()
        {
            return Rnd.NextDouble();
        }
    }
}