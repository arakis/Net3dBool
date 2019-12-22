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
using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Net3dBool
{
    /// <summary>
    /// Class representing a 3D solid.
    /// </summary>
    public class Solid
    {
        /** array of indices for the vertices from the 'vertices' attribute */
        protected int[] Indices;
        /** array of points defining the solid's vertices */
        protected Vector3[] Vertices;

        //--------------------------------CONSTRUCTORS----------------------------------//

        /** Constructs an empty solid. */
        public Solid()
        {
            SetInitialFeatures();
        }

        /**
        * Construct a solid based on data arrays. An exception may occur in the case of 
        * abnormal arrays (indices making references to inexistent vertices, there are less
        * colors than vertices...)
        * 
        * @param vertices array of points defining the solid vertices
        * @param indices array of indices for a array of vertices
        * @param colors array of colors defining the vertices colors 
        */
        public Solid(Vector3[] vertices, int[] indices)
            : this()
        {
            SetData(vertices, indices);
        }

        /** Sets the initial features common to all constructors */
        protected void SetInitialFeatures()
        {
            Vertices = new Vector3[0];
            Indices = new int[0];

            //            setCapability(Shape3D.ALLOW_GEOMETRY_WRITE);
            //            setCapability(Shape3D.ALLOW_APPEARANCE_WRITE);
            //            setCapability(Shape3D.ALLOW_APPEARANCE_READ);
        }

        //---------------------------------------GETS-----------------------------------//

        /**
        * Gets the solid vertices
        * 
        * @return solid vertices
        */
        public Vector3[] GetVertices()
        {
            Vector3[] newVertices = new Vector3[Vertices.Length];
            for (int i = 0; i < newVertices.Length; i++)
            {
                newVertices[i] = Vertices[i];
            }
            return newVertices;
        }

        /** Gets the solid indices for its vertices
        * 
        * @return solid indices for its vertices
        */
        public int[] GetIndices()
        {
            int[] newIndices = new int[Indices.Length];
            Array.Copy(Indices, 0, newIndices, 0, Indices.Length);
            return newIndices;
        }

        /**
        * Gets if the solid is empty (without any vertex)
        * 
        * @return true if the solid is empty, false otherwise
        */
        public bool IsEmpty => Indices.Length == 0;

        //---------------------------------------SETS-----------------------------------//

        /**
        * Sets the solid data. Each vertex may have a different color. An exception may 
        * occur in the case of abnormal arrays (e.g., indices making references to  
        * inexistent vertices, there are less colors than vertices...)
        * 
        * @param vertices array of points defining the solid vertices
        * @param indices array of indices for a array of vertices
        * @param colors array of colors defining the vertices colors 
        */
        public void SetData(Vector3[] vertices, int[] indices)
        {
            Vertices = new Vector3[vertices.Length];
            Indices = new int[indices.Length];
            if (indices.Length != 0)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vertices[i] = vertices[i];
                }
                Array.Copy(indices, 0, Indices, 0, indices.Length);

                DefineGeometry();
            }
        }

        //-------------------------GEOMETRICAL_TRANSFORMATIONS-------------------------//

        /**
        * Applies a translation into a solid
        * 
        * @param dx translation on the x axis
        * @param dy translation on the y axis
        */
        public void Translate(double dx, double dy)
        {
            if (dx != 0 || dy != 0)
            {
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].X += dx;
                    Vertices[i].Y += dy;
                }

                DefineGeometry();
            }
        }

        /**
        * Applies a rotation into a solid
        * 
        * @param dx rotation on the x axis
        * @param dy rotation on the y axis
        */
        public void Rotate(double dx, double dy)
        {
            double cosX = Math.Cos(dx);
            double cosY = Math.Cos(dy);
            double sinX = Math.Sin(dx);
            double sinY = Math.Sin(dy);

            if (dx != 0 || dy != 0)
            {
                //get mean
                Vector3 mean = GetMean();

                double newX, newY, newZ;
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].X -= mean.X;
                    Vertices[i].Y -= mean.Y;
                    Vertices[i].Z -= mean.Z;

                    //x rotation
                    if (dx != 0)
                    {
                        newY = Vertices[i].Y * cosX - Vertices[i].Z * sinX;
                        newZ = Vertices[i].Y * sinX + Vertices[i].Z * cosX;
                        Vertices[i].Y = newY;
                        Vertices[i].Z = newZ;
                    }

                    //y rotation
                    if (dy != 0)
                    {
                        newX = Vertices[i].X * cosY + Vertices[i].Z * sinY;
                        newZ = -Vertices[i].X * sinY + Vertices[i].Z * cosY;
                        Vertices[i].X = newX;
                        Vertices[i].Z = newZ;
                    }

                    Vertices[i].X += mean.X;
                    Vertices[i].Y += mean.Y;
                    Vertices[i].Z += mean.Z;
                }
            }

            DefineGeometry();
        }

        /**
        * Applies a zoom into a solid
        * 
        * @param dz translation on the z axis
        */
        public void Zoom(double dz)
        {
            if (dz != 0)
            {
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].Z += dz;
                }

                DefineGeometry();
            }
        }

        /**
        * Applies a scale changing into the solid
        * 
        * @param dx scale changing for the x axis 
        * @param dy scale changing for the y axis
        * @param dz scale changing for the z axis
        */
        public void Scale(double dx, double dy, double dz)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X *= dx;
                Vertices[i].Y *= dy;
                Vertices[i].Z *= dz;
            }

            DefineGeometry();
        }

        //-----------------------------------PRIVATES--------------------------------//

        /** Creates a geometry based on the indexes and vertices set for the solid */
        protected void DefineGeometry()
        {
            //            GeometryInfo gi = new GeometryInfo(GeometryInfo.TRIANGLE_ARRAY);
            //            gi.setCoordinateIndices(indices);
            //            gi.setCoordinates(vertices);
            //            NormalGenerator ng = new NormalGenerator();
            //            ng.generateNormals(gi);
            //
            //            gi.setColors(colors);
            //            gi.setColorIndices(indices);
            //            gi.recomputeIndices();
            //
            //            setGeometry(gi.getIndexedGeometryArray());
        }

        /**
        * Gets the solid mean
        * 
        * @return point representing the mean
        */
        protected Vector3 GetMean()
        {
            Vector3 mean = new Vector3();
            for (int i = 0; i < Vertices.Length; i++)
            {
                mean.X += Vertices[i].X;
                mean.Y += Vertices[i].Y;
                mean.Z += Vertices[i].Z;
            }
            mean.X /= Vertices.Length;
            mean.Y /= Vertices.Length;
            mean.Z /= Vertices.Length;

            return mean;
        }
    }
}

