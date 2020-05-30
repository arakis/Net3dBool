// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
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
using OpenToolkit.Mathematics;

namespace Net3dBool
{
    [Flags]
    public enum IntersectionType { None = 0, FrontFace = 1, BackFace = 2, Both = FrontFace | BackFace };

    /// <summary>
    /// a virtual ray that is casted from a begin Position in a certain Direction.
    /// </summary>
    public class Ray
    {
        public static double SameSurfaceOffset = .00001;

        public Vector3d Origin;
        public Vector3d DirectionNormal;
        public double MinDistanceToConsider;
        public double MaxDistanceToConsider;
        public Vector3d OneOverDirection;
        public bool IsShadowRay;
        public IntersectionType IntersectionType;

        public enum RaySign { negative = 1, positive = 0 };

        public RaySign[] Sign = new RaySign[3];

        public Ray(Vector3d origin, Vector3d directionNormal, double minDistanceToConsider = 0, double maxDistanceToConsider = double.PositiveInfinity, IntersectionType intersectionType = IntersectionType.FrontFace)
        {
            Origin = origin;
            DirectionNormal = directionNormal;
            MinDistanceToConsider = minDistanceToConsider;
            MaxDistanceToConsider = maxDistanceToConsider;
            IntersectionType = intersectionType;
            OneOverDirection = Vector3d.Divide(new Vector3d(1),directionNormal);

            Sign[0] = (OneOverDirection.X < 0) ? RaySign.negative : RaySign.positive;
            Sign[1] = (OneOverDirection.Y < 0) ? RaySign.negative : RaySign.positive;
            Sign[2] = (OneOverDirection.Z < 0) ? RaySign.negative : RaySign.positive;
        }

        public Ray(Ray rayToCopy)
        {
            Origin = rayToCopy.Origin;
            DirectionNormal = rayToCopy.DirectionNormal;
            MinDistanceToConsider = rayToCopy.MinDistanceToConsider;
            MaxDistanceToConsider = rayToCopy.MaxDistanceToConsider;
            OneOverDirection = rayToCopy.OneOverDirection;
            IsShadowRay = rayToCopy.IsShadowRay;
            IntersectionType = rayToCopy.IntersectionType;
            Sign[0] = rayToCopy.Sign[0];
            Sign[1] = rayToCopy.Sign[1];
            Sign[2] = rayToCopy.Sign[2];
        }

        public bool Intersection(AxisAlignedBoundingBox bounds)
        {
            Ray ray = this;
            // we calculate distance to the intersection with the x planes of the box
            double minDistFound = (bounds[(int)ray.Sign[0]].X - ray.Origin.X) * ray.OneOverDirection.X;
            double maxDistFound = (bounds[1 - (int)ray.Sign[0]].X - ray.Origin.X) * ray.OneOverDirection.X;

            // now find the distance to the y planes of the box
            double minDistToY = (bounds[(int)ray.Sign[1]].Y - ray.Origin.Y) * ray.OneOverDirection.Y;
            double maxDistToY = (bounds[1 - (int)ray.Sign[1]].Y - ray.Origin.Y) * ray.OneOverDirection.Y;

            if ((minDistFound > maxDistToY) || (minDistToY > maxDistFound))
            {
                return false;
            }

            if (minDistToY > minDistFound)
            {
                minDistFound = minDistToY;
            }

            if (maxDistToY < maxDistFound)
            {
                maxDistFound = maxDistToY;
            }

            // and finaly the z planes
            double minDistToZ = (bounds[(int)ray.Sign[2]].Z - ray.Origin.Z) * ray.OneOverDirection.Z;
            double maxDistToZ = (bounds[1 - (int)ray.Sign[2]].Z - ray.Origin.Z) * ray.OneOverDirection.Z;

            if ((minDistFound > maxDistToZ) || (minDistToZ > maxDistFound))
            {
                return false;
            }

            if (minDistToZ > minDistFound)
            {
                minDistFound = minDistToZ;
            }

            if (maxDistToZ < maxDistFound)
            {
                maxDistFound = maxDistToZ;
            }

            bool withinDistanceToConsider = (minDistFound < ray.MaxDistanceToConsider) && (maxDistFound > ray.MinDistanceToConsider);
            return withinDistanceToConsider;
        }
    }
}