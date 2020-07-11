
using OpenToolkit.Mathematics;

namespace Net3dBool
{
    public static class Vector3dExtensions 
    {
        /// <summary>
        /// Indicates whether this instance and a specified object are equal within an error range.
        /// </summary>
        /// <param name="OtherVector"></param>
        /// <param name="ErrorValue"></param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public static bool Equals(this Vector3d vector, Vector3d OtherVector, double ErrorValue)
        {
            if (vector.X < OtherVector.X + ErrorValue && vector.X > OtherVector.X - ErrorValue &&
                vector.Y < OtherVector.Y + ErrorValue && vector.Y > OtherVector.Y - ErrorValue &&
                vector.Z < OtherVector.Z + ErrorValue && vector.Z > OtherVector.Z - ErrorValue)
            {
                return true;
            }

            return false;
        }
    }
}