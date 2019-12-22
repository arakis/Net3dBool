/*
Copyright (c) 2014, Lars Brubaker
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System;

namespace Net3dBool
{
    public class AxisAlignedBoundingBox
    {
        public Vector3 MinXYZ;
        public Vector3 MaxXYZ;

        public AxisAlignedBoundingBox(Vector3 minXYZ, Vector3 maxXYZ)
        {
            if (maxXYZ.X < minXYZ.X || maxXYZ.Y < minXYZ.Y || maxXYZ.Z < minXYZ.Z)
            {
                throw new ArgumentException("All values of min must be less than all values in max.");
            }

            MinXYZ = minXYZ;
            MaxXYZ = maxXYZ;
        }

        public Vector3 Size
        {
            get
            {
                return MaxXYZ - MinXYZ;
            }
        }

        public double XSize
        {
            get
            {
                return MaxXYZ.X - MinXYZ.X;
            }
        }

        public double YSize
        {
            get
            {
                return MaxXYZ.Y - MinXYZ.Y;
            }
        }

        public double ZSize
        {
            get
            {
                return MaxXYZ.Z - MinXYZ.Z;
            }
        }

        /// <summary>
        /// Geth the corners by quadrant of the bottom
        /// </summary>
        /// <param name="quadrantIndex"></param>
        public Vector3 GetBottomCorner(int quadrantIndex)
        {
            switch (quadrantIndex)
            {
                case 0:
                    return new Vector3(MaxXYZ.X, MaxXYZ.Y, MinXYZ.Z);

                case 1:
                    return new Vector3(MinXYZ.X, MaxXYZ.Y, MinXYZ.Z);

                case 2:
                    return new Vector3(MinXYZ.X, MinXYZ.Y, MinXYZ.Z);

                case 3:
                    return new Vector3(MaxXYZ.X, MinXYZ.Y, MinXYZ.Z);
            }

            return Vector3.Zero;
        }

        public Vector3 Center
        {
            get
            {
                return (MinXYZ + MaxXYZ) / 2;
            }
        }

        /// <summary>
        /// This is the computation cost of doing an intersection with the given type.
        /// Attempt to give it in average CPU cycles for the intersecton.
        /// </summary>
        /// <returns></returns>
        public static double GetIntersectCost()
        {
            // it would be great to try and measure this more accurately.  This is a guess from looking at the intersect function.
            return 132;
        }

        public Vector3 GetCenter()
        {
            return (MinXYZ + MaxXYZ) * .5;
        }

        public double GetCenterX()
        {
            return (MinXYZ.X + MaxXYZ.X) * .5;
        }

        private double volumeCache = 0;

        public double GetVolume()
        {
            if (volumeCache == 0)
            {
                volumeCache = (MaxXYZ.X - MinXYZ.X) * (MaxXYZ.Y - MinXYZ.Y) * (MaxXYZ.Z - MinXYZ.Z);
            }

            return volumeCache;
        }

        private double SurfaceAreaCache = 0;

        public double GetSurfaceArea()
        {
            if (SurfaceAreaCache == 0)
            {
                double frontAndBack = (MaxXYZ.X - MinXYZ.X) * (MaxXYZ.Z - MinXYZ.Z) * 2;
                double leftAndRight = (MaxXYZ.Y - MinXYZ.Y) * (MaxXYZ.Z - MinXYZ.Z) * 2;
                double topAndBottom = (MaxXYZ.X - MinXYZ.X) * (MaxXYZ.Y - MinXYZ.Y) * 2;
                SurfaceAreaCache = frontAndBack + leftAndRight + topAndBottom;
            }

            return SurfaceAreaCache;
        }

        public Vector3 this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return MinXYZ;
                }
                else if (index == 1)
                {
                    return MaxXYZ;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public static AxisAlignedBoundingBox operator +(AxisAlignedBoundingBox A, AxisAlignedBoundingBox B)
        {
#if true
            return Union(A, B);
#else
            Vector3 calcMinXYZ = new Vector3();
            calcMinXYZ.x = Math.Min(A.minXYZ.x, B.minXYZ.x);
            calcMinXYZ.y = Math.Min(A.minXYZ.y, B.minXYZ.y);
            calcMinXYZ.z = Math.Min(A.minXYZ.z, B.minXYZ.z);

            Vector3 calcMaxXYZ = new Vector3();
            calcMaxXYZ.x = Math.Max(A.maxXYZ.x, B.maxXYZ.x);
            calcMaxXYZ.y = Math.Max(A.maxXYZ.y, B.maxXYZ.y);
            calcMaxXYZ.z = Math.Max(A.maxXYZ.z, B.maxXYZ.z);

            AxisAlignedBoundingBox combinedBounds = new AxisAlignedBoundingBox(calcMinXYZ, calcMaxXYZ);

            return combinedBounds;
#endif
        }

        public static AxisAlignedBoundingBox Union(AxisAlignedBoundingBox boundsA, AxisAlignedBoundingBox boundsB)
        {
            Vector3 minXYZ = Vector3.Zero;
            minXYZ.X = Math.Min(boundsA.MinXYZ.X, boundsB.MinXYZ.X);
            minXYZ.Y = Math.Min(boundsA.MinXYZ.Y, boundsB.MinXYZ.Y);
            minXYZ.Z = Math.Min(boundsA.MinXYZ.Z, boundsB.MinXYZ.Z);

            Vector3 maxXYZ = Vector3.Zero;
            maxXYZ.X = Math.Max(boundsA.MaxXYZ.X, boundsB.MaxXYZ.X);
            maxXYZ.Y = Math.Max(boundsA.MaxXYZ.Y, boundsB.MaxXYZ.Y);
            maxXYZ.Z = Math.Max(boundsA.MaxXYZ.Z, boundsB.MaxXYZ.Z);

            return new AxisAlignedBoundingBox(minXYZ, maxXYZ);
        }

        public static AxisAlignedBoundingBox Intersection(AxisAlignedBoundingBox boundsA, AxisAlignedBoundingBox boundsB)
        {
            Vector3 minXYZ = Vector3.Zero;
            minXYZ.X = Math.Max(boundsA.MinXYZ.X, boundsB.MinXYZ.X);
            minXYZ.Y = Math.Max(boundsA.MinXYZ.Y, boundsB.MinXYZ.Y);
            minXYZ.Z = Math.Max(boundsA.MinXYZ.Z, boundsB.MinXYZ.Z);

            Vector3 maxXYZ = Vector3.Zero;
            maxXYZ.X = Math.Max(minXYZ.X, Math.Min(boundsA.MaxXYZ.X, boundsB.MaxXYZ.X));
            maxXYZ.Y = Math.Max(minXYZ.Y, Math.Min(boundsA.MaxXYZ.Y, boundsB.MaxXYZ.Y));
            maxXYZ.Z = Math.Max(minXYZ.Z, Math.Min(boundsA.MaxXYZ.Z, boundsB.MaxXYZ.Z));

            return new AxisAlignedBoundingBox(minXYZ, maxXYZ);
        }

        public static AxisAlignedBoundingBox Union(AxisAlignedBoundingBox bounds, Vector3 vertex)
        {
            Vector3 minXYZ = Vector3.Zero;
            minXYZ.X = Math.Min(bounds.MinXYZ.X, vertex.X);
            minXYZ.Y = Math.Min(bounds.MinXYZ.Y, vertex.Y);
            minXYZ.Z = Math.Min(bounds.MinXYZ.Z, vertex.Z);

            Vector3 maxXYZ = Vector3.Zero;
            maxXYZ.X = Math.Max(bounds.MaxXYZ.X, vertex.X);
            maxXYZ.Y = Math.Max(bounds.MaxXYZ.Y, vertex.Y);
            maxXYZ.Z = Math.Max(bounds.MaxXYZ.Z, vertex.Z);

            return new AxisAlignedBoundingBox(minXYZ, maxXYZ);
        }

        public void Clamp(ref Vector3 positionToClamp)
        {
            if (positionToClamp.X < MinXYZ.X)
            {
                positionToClamp.X = MinXYZ.X;
            }
            else if (positionToClamp.X > MaxXYZ.X)
            {
                positionToClamp.X = MaxXYZ.X;
            }

            if (positionToClamp.Y < MinXYZ.Y)
            {
                positionToClamp.Y = MinXYZ.Y;
            }
            else if (positionToClamp.Y > MaxXYZ.Y)
            {
                positionToClamp.Y = MaxXYZ.Y;
            }

            if (positionToClamp.Z < MinXYZ.Z)
            {
                positionToClamp.Z = MinXYZ.Z;
            }
            else if (positionToClamp.Z > MaxXYZ.Z)
            {
                positionToClamp.Z = MaxXYZ.Z;
            }
        }

        public bool Contains(AxisAlignedBoundingBox bounds)
        {
            if (MinXYZ.X <= bounds.MinXYZ.X
                && MaxXYZ.X >= bounds.MaxXYZ.X
                && MinXYZ.Y <= bounds.MinXYZ.Y
                && MaxXYZ.Y >= bounds.MaxXYZ.Y
                && MinXYZ.Z <= bounds.MinXYZ.Z
                && MaxXYZ.Z >= bounds.MaxXYZ.Z)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("min {0} - max {1}", MinXYZ, MaxXYZ);
        }
    }
}