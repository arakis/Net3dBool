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

using OpenToolkit.Mathematics;

namespace Net3dBool
{
    public class Plane
    {
        public double DistanceToPlaneFromOrigin;
        public Vector3d PlaneNormal;
        private const double TreatAsZero = .000000001;

        public Plane(Vector3d planeNormal, double distanceFromOrigin)
        {
            PlaneNormal = planeNormal.Normalized();
            DistanceToPlaneFromOrigin = distanceFromOrigin;
        }

        public Plane(Vector3d point0, Vector3d point1, Vector3d point2)
        {
            PlaneNormal = Vector3d.Cross((point1 - point0), (point2 - point0)).Normalized();
            DistanceToPlaneFromOrigin = Vector3d.Dot(PlaneNormal, point0);
        }

        public Plane(Vector3d planeNormal, Vector3d pointOnPlane)
        {
            PlaneNormal = planeNormal.Normalized();
            DistanceToPlaneFromOrigin = Vector3d.Dot(planeNormal, pointOnPlane);
        }

        public double GetDistanceFromPlane(Vector3d positionToCheck)
        {
            double distanceToPointFromOrigin = Vector3d.Dot(positionToCheck, PlaneNormal);
            return distanceToPointFromOrigin - DistanceToPlaneFromOrigin;
        }

        public double GetDistanceToIntersection(Ray ray, out bool inFront)
        {
            inFront = false;
            double normalDotRayDirection = Vector3d.Dot(PlaneNormal, ray.DirectionNormal);
            if (normalDotRayDirection < TreatAsZero && normalDotRayDirection > -TreatAsZero) // the ray is parallel to the plane
            {
                return double.PositiveInfinity;
            }

            if (normalDotRayDirection < 0)
            {
                inFront = true;
            }

            return (DistanceToPlaneFromOrigin - Vector3d.Dot(PlaneNormal, ray.Origin)) / normalDotRayDirection;
        }

        public double GetDistanceToIntersection(Vector3d pointOnLine, Vector3d lineDirection)
        {
            double normalDotRayDirection = Vector3d.Dot(PlaneNormal, lineDirection);
            if (normalDotRayDirection < TreatAsZero && normalDotRayDirection > -TreatAsZero) // the ray is parallel to the plane
            {
                return double.PositiveInfinity;
            }

            double planeNormalDotPointOnLine = Vector3d.Dot(PlaneNormal, pointOnLine);
            return (DistanceToPlaneFromOrigin - planeNormalDotPointOnLine) / normalDotRayDirection;
        }

        public bool RayHitPlane(Ray ray, out double distanceToHit, out bool hitFrontOfPlane)
        {
            distanceToHit = double.PositiveInfinity;
            hitFrontOfPlane = false;

            double normalDotRayDirection = Vector3d.Dot(PlaneNormal, ray.DirectionNormal);
            if (normalDotRayDirection < TreatAsZero && normalDotRayDirection > -TreatAsZero) // the ray is parallel to the plane
            {
                return false;
            }

            if (normalDotRayDirection < 0)
            {
                hitFrontOfPlane = true;
            }

            double distanceToRayOriginFromOrigin = Vector3d.Dot(PlaneNormal, ray.Origin);

            double distanceToPlaneFromRayOrigin = DistanceToPlaneFromOrigin - distanceToRayOriginFromOrigin;

            bool originInFrontOfPlane = distanceToPlaneFromRayOrigin < 0;

            bool originAndHitAreOnSameSide = originInFrontOfPlane == hitFrontOfPlane;
            if (!originAndHitAreOnSameSide)
            {
                return false;
            }

            distanceToHit = distanceToPlaneFromRayOrigin / normalDotRayDirection;
            return true;
        }

        public bool LineHitPlane(Vector3d start, Vector3d end, out Vector3d intersectionPosition)
        {
            double distanceToStartFromOrigin = Vector3d.Dot(PlaneNormal, start);
            if (distanceToStartFromOrigin == 0)
            {
                intersectionPosition = start;
                return true;
            }

            double distanceToEndFromOrigin = Vector3d.Dot(PlaneNormal, end);
            if (distanceToEndFromOrigin == 0)
            {
                intersectionPosition = end;
                return true;
            }

            if ((distanceToStartFromOrigin < 0 && distanceToEndFromOrigin > 0)
                || (distanceToStartFromOrigin > 0 && distanceToEndFromOrigin < 0))
            {
                Vector3d direction = (end - start).Normalized();

                double startDistanceFromPlane = distanceToStartFromOrigin - DistanceToPlaneFromOrigin;
                double endDistanceFromPlane = distanceToEndFromOrigin - DistanceToPlaneFromOrigin;
                double lengthAlongPlanNormal = endDistanceFromPlane - startDistanceFromPlane;

                double ratioToPlanFromStart = startDistanceFromPlane / lengthAlongPlanNormal;
                intersectionPosition = start + direction * ratioToPlanFromStart;

                return true;
            }

            intersectionPosition = Vector3d.PositiveInfinity;
            return false;
        }
    }
}