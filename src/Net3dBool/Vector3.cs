// #region --- License ---

// /*
// Copyright (c) 2006 - 2008 The Open Toolkit library.

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// */

// #endregion --- License ---

// using System;
// using System.Runtime.InteropServices;

// namespace Net3dBool
// {
//     /// <summary>
//     /// Represents a 3D vector using three double-precision floating-point numbers.
//     /// </summary>
//     [Serializable]
//     [StructLayout(LayoutKind.Sequential)]
//     public struct Vector3d : IEquatable<Vector3d>
//     {
//         #region Fields

//         /// <summary>
//         /// The X component of the Vector3.
//         /// </summary>
//         public double X;

//         /// <summary>
//         /// The Y component of the Vector3.
//         /// </summary>
//         public double Y;

//         /// <summary>
//         /// The Z component of the Vector3.
//         /// </summary>
//         public double Z;

//         #endregion Fields

//         #region Constructors

//         /// <summary>
//         /// Constructs a new Vector3.
//         /// </summary>
//         /// <param name="x">The x component of the Vector3.</param>
//         /// <param name="y">The y component of the Vector3.</param>
//         /// <param name="z">The z component of the Vector3.</param>
//         public Vector3d(double x, double y, double z)
//         {
//             X = x;
//             Y = y;
//             Z = z;
//         }

//         /// <summary>
//         /// Constructs a new instance from the given Vector3d.
//         /// </summary>
//         /// <param name="v">The Vector3d to copy components from.</param>
//         public Vector3d(Vector3d v)
//         {
//             X = v.X;
//             Y = v.Y;
//             Z = v.Z;
//         }

//         public Vector3d(double[] doubleArray)
//         {
//             X = doubleArray[0];
//             Y = doubleArray[1];
//             Z = doubleArray[2];
//         }

//         #endregion Constructors

//         #region Properties

//         public double this[int index]
//         {
//             get
//             {
//                 switch (index)
//                 {
//                     case 0:
//                         return X;

//                     case 1:
//                         return Y;

//                     case 2:
//                         return Z;

//                     default:
//                         return 0;
//                 }
//             }

//             set
//             {
//                 switch (index)
//                 {
//                     case 0:
//                         X = value;
//                         break;

//                     case 1:
//                         Y = value;
//                         break;

//                     case 2:
//                         Z = value;
//                         break;

//                     default:
//                         throw new Exception();
//                 }
//             }
//         }

//         #endregion Properties

//         #region Public Members

//         #region Instance

//         #region public double Length

//         /// <summary>
//         /// Gets the length (magnitude) of the vector.
//         /// </summary>
//         /// <see cref="LengthFast"/>
//         /// <seealso cref="LengthSquared"/>
//         public double Length
//         {
//             get
//             {
//                 return Math.Sqrt(X * X + Y * Y + Z * Z);
//             }
//         }

//         #endregion public double Length

//         #region public double LengthSquared

//         /// <summary>
//         /// Gets the square of the vector length (magnitude).
//         /// </summary>
//         /// <remarks>
//         /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
//         /// for comparisons.
//         /// </remarks>
//         /// <see cref="Length"/>
//         /// <seealso cref="LengthFast"/>
//         public double LengthSquared
//         {
//             get
//             {
//                 return X * X + Y * Y + Z * Z;
//             }
//         }

//         #endregion public double LengthSquared

//         #region public void Normalize()

//         /// <summary>
//         /// Returns a normalized Vector of this.
//         /// </summary>
//         /// <returns></returns>
//         public Vector3d GetNormal()
//         {
//             Vector3d temp = this;
//             temp.Normalize();
//             return temp;
//         }

//         /// <summary>
//         /// Scales the Vector3d to unit length.
//         /// </summary>
//         public void Normalize()
//         {
//             double scale = 1.0 / Length;
//             X *= scale;
//             Y *= scale;
//             Z *= scale;
//         }

//         #endregion public void Normalize()

//         #region public double[] ToArray()

//         public double[] ToArray()
//         {
//             return new double[] { X, Y, Z };
//         }

//         #endregion public double[] ToArray()

//         #endregion Instance

//         #region Static

//         #region Fields

//         /// <summary>
//         /// Defines a unit-length Vector3d that points towards the X-axis.
//         /// </summary>
//         public static readonly Vector3d UnitX = new Vector3d(1, 0, 0);

//         /// <summary>
//         /// Defines a unit-length Vector3d that points towards the Y-axis.
//         /// </summary>
//         public static readonly Vector3d UnitY = new Vector3d(0, 1, 0);

//         /// <summary>
//         /// /// Defines a unit-length Vector3d that points towards the Z-axis.
//         /// </summary>
//         public static readonly Vector3d UnitZ = new Vector3d(0, 0, 1);

//         /// <summary>
//         /// Defines a zero-length Vector3.
//         /// </summary>
//         public static readonly Vector3d Zero = new Vector3d(0, 0, 0);

//         /// <summary>
//         /// Defines an instance with all components set to 1.
//         /// </summary>
//         public static readonly Vector3d One = new Vector3d(1, 1, 1);

//         /// <summary>
//         /// Defines an instance with all components set to positive infinity.
//         /// </summary>
//         public static readonly Vector3d PositiveInfinity = new Vector3d(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

//         /// <summary>
//         /// Defines an instance with all components set to negative infinity.
//         /// </summary>
//         public static readonly Vector3d NegativeInfinity = new Vector3d(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

//         /// <summary>
//         /// Defines the size of the Vector3d struct in bytes.
//         /// </summary>
//         public static readonly int SizeInBytes = Marshal.SizeOf(new Vector3d());

//         #endregion Fields

//         #region Add

//         /// <summary>
//         /// Adds two vectors.
//         /// </summary>
//         /// <param name="a">Left operand.</param>
//         /// <param name="b">Right operand.</param>
//         /// <returns>Result of operation.</returns>
//         public static Vector3d Add(Vector3d a, Vector3d b)
//         {
//             Add(ref a, ref b, out a);
//             return a;
//         }

//         /// <summary>
//         /// Adds two vectors.
//         /// </summary>
//         /// <param name="a">Left operand.</param>
//         /// <param name="b">Right operand.</param>
//         /// <param name="result">Result of operation.</param>
//         public static void Add(ref Vector3d a, ref Vector3d b, out Vector3d result)
//         {
//             result = new Vector3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
//         }

//         #endregion Add

//         #region Subtract

//         /// <summary>
//         /// Subtract one Vector from another
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <returns>Result of subtraction</returns>
//         public static Vector3d Subtract(Vector3d a, Vector3d b)
//         {
//             Subtract(ref a, ref b, out a);
//             return a;
//         }

//         /// <summary>
//         /// Subtract one Vector from another
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <param name="result">Result of subtraction</param>
//         public static void Subtract(ref Vector3d a, ref Vector3d b, out Vector3d result)
//         {
//             result = new Vector3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
//         }

//         #endregion Subtract

//         #region Multiply

//         /// <summary>
//         /// Multiplies a vector by a scalar.
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <returns>Result of the operation.</returns>
//         public static Vector3d Multiply(Vector3d vector, double scale)
//         {
//             Multiply(ref vector, scale, out vector);
//             return vector;
//         }

//         /// <summary>
//         /// Multiplies a vector by a scalar.
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <param name="result">Result of the operation.</param>
//         public static void Multiply(ref Vector3d vector, double scale, out Vector3d result)
//         {
//             result = new Vector3d(vector.X * scale, vector.Y * scale, vector.Z * scale);
//         }

//         /// <summary>
//         /// Multiplies a vector by the components a vector (scale).
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <returns>Result of the operation.</returns>
//         public static Vector3d Multiply(Vector3d vector, Vector3d scale)
//         {
//             Multiply(ref vector, ref scale, out vector);
//             return vector;
//         }

//         /// <summary>
//         /// Multiplies a vector by the components of a vector (scale).
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <param name="result">Result of the operation.</param>
//         public static void Multiply(ref Vector3d vector, ref Vector3d scale, out Vector3d result)
//         {
//             result = new Vector3d(vector.X * scale.X, vector.Y * scale.Y, vector.Z * scale.Z);
//         }

//         #endregion Multiply

//         #region Divide

//         /// <summary>
//         /// Divides a vector by a scalar.
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <returns>Result of the operation.</returns>
//         public static Vector3d Divide(Vector3d vector, double scale)
//         {
//             Divide(ref vector, scale, out vector);
//             return vector;
//         }

//         /// <summary>
//         /// Divides a vector by a scalar.
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <param name="result">Result of the operation.</param>
//         public static void Divide(ref Vector3d vector, double scale, out Vector3d result)
//         {
//             Multiply(ref vector, 1 / scale, out result);
//         }

//         /// <summary>
//         /// Divides a vector by the components of a vector (scale).
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <returns>Result of the operation.</returns>
//         public static Vector3d Divide(Vector3d vector, Vector3d scale)
//         {
//             Divide(ref vector, ref scale, out vector);
//             return vector;
//         }

//         /// <summary>
//         /// Divide a vector by the components of a vector (scale).
//         /// </summary>
//         /// <param name="vector">Left operand.</param>
//         /// <param name="scale">Right operand.</param>
//         /// <param name="result">Result of the operation.</param>
//         public static void Divide(ref Vector3d vector, ref Vector3d scale, out Vector3d result)
//         {
//             result = new Vector3d(vector.X / scale.X, vector.Y / scale.Y, vector.Z / scale.Z);
//         }

//         #endregion Divide

//         #region ComponentMin

//         /// <summary>
//         /// Calculate the component-wise minimum of two vectors
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <returns>The component-wise minimum</returns>
//         public static Vector3d ComponentMin(Vector3d a, Vector3d b)
//         {
//             a.X = a.X < b.X ? a.X : b.X;
//             a.Y = a.Y < b.Y ? a.Y : b.Y;
//             a.Z = a.Z < b.Z ? a.Z : b.Z;
//             return a;
//         }

//         /// <summary>
//         /// Calculate the component-wise minimum of two vectors
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <param name="result">The component-wise minimum</param>
//         public static void ComponentMin(ref Vector3d a, ref Vector3d b, out Vector3d result)
//         {
//             result.X = a.X < b.X ? a.X : b.X;
//             result.Y = a.Y < b.Y ? a.Y : b.Y;
//             result.Z = a.Z < b.Z ? a.Z : b.Z;
//         }

//         #endregion ComponentMin

//         #region ComponentMax

//         /// <summary>
//         /// Calculate the component-wise maximum of two vectors
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <returns>The component-wise maximum</returns>
//         public static Vector3d ComponentMax(Vector3d a, Vector3d b)
//         {
//             a.X = a.X > b.X ? a.X : b.X;
//             a.Y = a.Y > b.Y ? a.Y : b.Y;
//             a.Z = a.Z > b.Z ? a.Z : b.Z;
//             return a;
//         }

//         /// <summary>
//         /// Calculate the component-wise maximum of two vectors
//         /// </summary>
//         /// <param name="a">First operand</param>
//         /// <param name="b">Second operand</param>
//         /// <param name="result">The component-wise maximum</param>
//         public static void ComponentMax(ref Vector3d a, ref Vector3d b, out Vector3d result)
//         {
//             result.X = a.X > b.X ? a.X : b.X;
//             result.Y = a.Y > b.Y ? a.Y : b.Y;
//             result.Z = a.Z > b.Z ? a.Z : b.Z;
//         }

//         #endregion ComponentMax

//         #region Min

//         /// <summary>
//         /// Returns the Vector3d with the minimum magnitude
//         /// </summary>
//         /// <param name="left">Left operand</param>
//         /// <param name="right">Right operand</param>
//         /// <returns>The minimum Vector3</returns>
//         public static Vector3d Min(Vector3d left, Vector3d right)
//         {
//             return left.LengthSquared < right.LengthSquared ? left : right;
//         }

//         #endregion Min

//         #region Max

//         /// <summary>
//         /// Returns the Vector3d with the minimum magnitude
//         /// </summary>
//         /// <param name="left">Left operand</param>
//         /// <param name="right">Right operand</param>
//         /// <returns>The minimum Vector3</returns>
//         public static Vector3d Max(Vector3d left, Vector3d right)
//         {
//             return left.LengthSquared >= right.LengthSquared ? left : right;
//         }

//         #endregion Max

//         #region Clamp

//         /// <summary>
//         /// Clamp a vector to the given minimum and maximum vectors
//         /// </summary>
//         /// <param name="vec">Input vector</param>
//         /// <param name="min">Minimum vector</param>
//         /// <param name="max">Maximum vector</param>
//         /// <returns>The clamped vector</returns>
//         public static Vector3d Clamp(Vector3d vec, Vector3d min, Vector3d max)
//         {
//             vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//             vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//             vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//             return vec;
//         }

//         /// <summary>
//         /// Clamp a vector to the given minimum and maximum vectors
//         /// </summary>
//         /// <param name="vec">Input vector</param>
//         /// <param name="min">Minimum vector</param>
//         /// <param name="max">Maximum vector</param>
//         /// <param name="result">The clamped vector</param>
//         public static void Clamp(ref Vector3d vec, ref Vector3d min, ref Vector3d max, out Vector3d result)
//         {
//             result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
//             result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
//             result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
//         }

//         #endregion Clamp

//         #region Normalize

//         /// <summary>
//         /// Scale a vector to unit length
//         /// </summary>
//         /// <param name="vec">The input vector</param>
//         /// <returns>The normalized vector</returns>
//         public static Vector3d Normalize(Vector3d vec)
//         {
//             double scale = 1.0 / vec.Length;
//             vec.X *= scale;
//             vec.Y *= scale;
//             vec.Z *= scale;
//             return vec;
//         }

//         /// <summary>
//         /// Scale a vector to unit length
//         /// </summary>
//         /// <param name="vec">The input vector</param>
//         /// <param name="result">The normalized vector</param>
//         public static void Normalize(ref Vector3d vec, out Vector3d result)
//         {
//             double scale = 1.0 / vec.Length;
//             result.X = vec.X * scale;
//             result.Y = vec.Y * scale;
//             result.Z = vec.Z * scale;
//         }

//         #endregion Normalize

//         #region Dot

//         /// <summary>
//         /// Calculate the dot (scalar) product of two vectors
//         /// </summary>
//         /// <param name="left">First operand</param>
//         /// <param name="right">Second operand</param>
//         /// <returns>The dot product of the two inputs</returns>
//         public static double Dot(Vector3d left, Vector3d right)
//         {
//             return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
//         }

//         /// <summary>
//         /// Calculate the dot (scalar) product of two vectors
//         /// </summary>
//         /// <param name="left">First operand</param>
//         /// <param name="right">Second operand</param>
//         /// <param name="result">The dot product of the two inputs</param>
//         public static void Dot(ref Vector3d left, ref Vector3d right, out double result)
//         {
//             result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
//         }

//         #endregion Dot

//         #region Cross

//         /// <summary>
//         /// Caclulate the cross (vector) product of two vectors
//         /// </summary>
//         /// <param name="left">First operand</param>
//         /// <param name="right">Second operand</param>
//         /// <returns>The cross product of the two inputs</returns>
//         public static Vector3d Cross(Vector3d left, Vector3d right)
//         {
//             Vector3d result;
//             Cross(ref left, ref right, out result);
//             return result;
//         }

//         /// <summary>
//         /// Caclulate the cross (vector) product of two vectors
//         /// </summary>
//         /// <param name="left">First operand</param>
//         /// <param name="right">Second operand</param>
//         /// <returns>The cross product of the two inputs</returns>
//         /// <param name="result">The cross product of the two inputs</param>
//         public static void Cross(ref Vector3d left, ref Vector3d right, out Vector3d result)
//         {
//             result = new Vector3d(left.Y * right.Z - left.Z * right.Y,
//                 left.Z * right.X - left.X * right.Z,
//                 left.X * right.Y - left.Y * right.X);
//         }

//         #endregion Cross

//         #region Utility

//         /// <summary>
//         /// Checks if 3 points are collinear (all lie on the same line).
//         /// </summary>
//         /// <param name="a"></param>
//         /// <param name="b"></param>
//         /// <param name="c"></param>
//         /// <param name="epsilon"></param>
//         /// <returns></returns>
//         public static bool Collinear(Vector3d a, Vector3d b, Vector3d c, double epsilon = .000001)
//         {
//             // Return true if a, b, and c all lie on the same line.
//             return Math.Abs(Cross(b - a, c - a).Length) < epsilon;
//         }

//         public static Vector3d GetPerpendicular(Vector3d a, Vector3d b)
//         {
//             if (!Collinear(a, b, Zero))
//             {
//                 return Cross(a, b);
//             }
//             else
//             {
//                 Vector3d zOne = new Vector3d(0, 0, 100000);
//                 if (!Collinear(a, b, zOne))
//                 {
//                     return Cross(a - zOne, b - zOne);
//                 }
//                 else
//                 {
//                     Vector3d xOne = new Vector3d(1000000, 0, 0);
//                     return Cross(a - xOne, b - xOne);
//                 }
//             }
//         }

//         #endregion Utility

//         #region Lerp

//         /// <summary>
//         /// Returns a new Vector that is the linear blend of the 2 given Vectors
//         /// </summary>
//         /// <param name="a">First input vector</param>
//         /// <param name="b">Second input vector</param>
//         /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//         /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
//         public static Vector3d Lerp(Vector3d a, Vector3d b, double blend)
//         {
//             a.X = blend * (b.X - a.X) + a.X;
//             a.Y = blend * (b.Y - a.Y) + a.Y;
//             a.Z = blend * (b.Z - a.Z) + a.Z;
//             return a;
//         }

//         /// <summary>
//         /// Returns a new Vector that is the linear blend of the 2 given Vectors
//         /// </summary>
//         /// <param name="a">First input vector</param>
//         /// <param name="b">Second input vector</param>
//         /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
//         /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
//         public static void Lerp(ref Vector3d a, ref Vector3d b, double blend, out Vector3d result)
//         {
//             result.X = blend * (b.X - a.X) + a.X;
//             result.Y = blend * (b.Y - a.Y) + a.Y;
//             result.Z = blend * (b.Z - a.Z) + a.Z;
//         }

//         #endregion Lerp

//         #region Barycentric

//         /// <summary>
//         /// Interpolate 3 Vectors using Barycentric coordinates
//         /// </summary>
//         /// <param name="a">First input Vector</param>
//         /// <param name="b">Second input Vector</param>
//         /// <param name="c">Third input Vector</param>
//         /// <param name="u">First Barycentric Coordinate</param>
//         /// <param name="v">Second Barycentric Coordinate</param>
//         /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
//         public static Vector3d BaryCentric(Vector3d a, Vector3d b, Vector3d c, double u, double v)
//         {
//             return a + u * (b - a) + v * (c - a);
//         }

//         /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
//         /// <param name="a">First input Vector.</param>
//         /// <param name="b">Second input Vector.</param>
//         /// <param name="c">Third input Vector.</param>
//         /// <param name="u">First Barycentric Coordinate.</param>
//         /// <param name="v">Second Barycentric Coordinate.</param>
//         /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
//         public static void BaryCentric(ref Vector3d a, ref Vector3d b, ref Vector3d c, double u, double v, out Vector3d result)
//         {
//             result = a; // copy

//             Vector3d temp = b; // copy
//             Subtract(ref temp, ref a, out temp);
//             Multiply(ref temp, u, out temp);
//             Add(ref result, ref temp, out result);

//             temp = c; // copy
//             Subtract(ref temp, ref a, out temp);
//             Multiply(ref temp, v, out temp);
//             Add(ref result, ref temp, out result);
//         }

//         #endregion Barycentric

//         #region CalculateAngle

//         /// <summary>
//         /// Calculates the angle (in radians) between two vectors.
//         /// </summary>
//         /// <param name="first">The first vector.</param>
//         /// <param name="second">The second vector.</param>
//         /// <returns>Angle (in radians) between the vectors.</returns>
//         /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
//         public static double CalculateAngle(Vector3d first, Vector3d second)
//         {
//             return Math.Acos((Dot(first, second)) / (first.Length * second.Length));
//         }

//         /// <summary>Calculates the angle (in radians) between two vectors.</summary>
//         /// <param name="first">The first vector.</param>
//         /// <param name="second">The second vector.</param>
//         /// <param name="result">Angle (in radians) between the vectors.</param>
//         /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
//         public static void CalculateAngle(ref Vector3d first, ref Vector3d second, out double result)
//         {
//             double temp;
//             Dot(ref first, ref second, out temp);
//             result = Math.Acos(temp / (first.Length * second.Length));
//         }

//         #endregion CalculateAngle

//         #endregion Static

//         #region Operators

//         /// <summary>
//         /// Adds two instances.
//         /// </summary>
//         /// <param name="left">The first instance.</param>
//         /// <param name="right">The second instance.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator +(Vector3d left, Vector3d right)
//         {
//             left.X += right.X;
//             left.Y += right.Y;
//             left.Z += right.Z;
//             return left;
//         }

//         /// <summary>
//         /// Subtracts two instances.
//         /// </summary>
//         /// <param name="left">The first instance.</param>
//         /// <param name="right">The second instance.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator -(Vector3d left, Vector3d right)
//         {
//             left.X -= right.X;
//             left.Y -= right.Y;
//             left.Z -= right.Z;
//             return left;
//         }

//         /// <summary>
//         /// Negates an instance.
//         /// </summary>
//         /// <param name="vec">The instance.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator -(Vector3d vec)
//         {
//             vec.X = -vec.X;
//             vec.Y = -vec.Y;
//             vec.Z = -vec.Z;
//             return vec;
//         }

//         /// <summary>
//         /// Component wise multiply two vectors together, x*x, y*y, z*z.
//         /// </summary>
//         /// <param name="vecA"></param>
//         /// <param name="vecB"></param>
//         /// <returns></returns>
//         public static Vector3d operator *(Vector3d vecA, Vector3d vecB)
//         {
//             vecA.X *= vecB.X;
//             vecA.Y *= vecB.Y;
//             vecA.Z *= vecB.Z;
//             return vecA;
//         }

//         /// <summary>
//         /// Multiplies an instance by a scalar.
//         /// </summary>
//         /// <param name="vec">The instance.</param>
//         /// <param name="scale">The scalar.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator *(Vector3d vec, double scale)
//         {
//             vec.X *= scale;
//             vec.Y *= scale;
//             vec.Z *= scale;
//             return vec;
//         }

//         /// <summary>
//         /// Multiplies an instance by a scalar.
//         /// </summary>
//         /// <param name="scale">The scalar.</param>
//         /// <param name="vec">The instance.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator *(double scale, Vector3d vec)
//         {
//             vec.X *= scale;
//             vec.Y *= scale;
//             vec.Z *= scale;
//             return vec;
//         }

//         /// <summary>
//         /// Creates a new vector which is the numerator devided by each component of the vector.
//         /// </summary>
//         /// <param name="numerator"></param>
//         /// <param name="vec"></param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator /(double numerator, Vector3d vec)
//         {
//             return new Vector3d((numerator / vec.X), (numerator / vec.Y), (numerator / vec.Z));
//         }

//         /// <summary>
//         /// Divides an instance by a scalar.
//         /// </summary>
//         /// <param name="vec">The instance.</param>
//         /// <param name="scale">The scalar.</param>
//         /// <returns>The result of the calculation.</returns>
//         public static Vector3d operator /(Vector3d vec, double scale)
//         {
//             double mult = 1 / scale;
//             vec.X *= mult;
//             vec.Y *= mult;
//             vec.Z *= mult;
//             return vec;
//         }

//         /// <summary>
//         /// Compares two instances for equality.
//         /// </summary>
//         /// <param name="left">The first instance.</param>
//         /// <param name="right">The second instance.</param>
//         /// <returns>True, if left equals right; false otherwise.</returns>
//         public static bool operator ==(Vector3d left, Vector3d right)
//         {
//             return left.Equals(right);
//         }

//         /// <summary>
//         /// Compares two instances for inequality.
//         /// </summary>
//         /// <param name="left">The first instance.</param>
//         /// <param name="right">The second instance.</param>
//         /// <returns>True, if left does not equa lright; false otherwise.</returns>
//         public static bool operator !=(Vector3d left, Vector3d right)
//         {
//             return !left.Equals(right);
//         }

//         #endregion Operators

//         #region Overrides

//         #region public override string ToString()

//         /// <summary>
//         /// Returns a System.String that represents the current Vector3.
//         /// </summary>
//         /// <returns></returns>
//         public override string ToString()
//         {
//             return String.Format("[{0}, {1}, {2}]", X, Y, Z);
//         }

//         #endregion public override string ToString()

//         #region public override int GetHashCode()

//         /// <summary>
//         /// Returns the hashcode for this instance.
//         /// </summary>
//         /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
//         public override int GetHashCode()
//         {
//             return new { X, Y, Z }.GetHashCode();
//         }

//         #endregion public override int GetHashCode()

//         #region public override bool Equals(object obj)

//         /// <summary>
//         /// Indicates whether this instance and a specified object are equal.
//         /// </summary>
//         /// <param name="obj">The object to compare to.</param>
//         /// <returns>True if the instances are equal; false otherwise.</returns>
//         public override bool Equals(object obj)
//         {
//             if (!(obj is Vector3d))
//                 return false;

//             return Equals((Vector3d)obj);
//         }

//         /// <summary>
//         /// Indicates whether this instance and a specified object are equal within an error range.
//         /// </summary>
//         /// <param name="OtherVector"></param>
//         /// <param name="ErrorValue"></param>
//         /// <returns>True if the instances are equal; false otherwise.</returns>
//         public bool Equals(Vector3d OtherVector, double ErrorValue)
//         {
//             if ((X < OtherVector.X + ErrorValue && X > OtherVector.X - ErrorValue) &&
//                 (Y < OtherVector.Y + ErrorValue && Y > OtherVector.Y - ErrorValue) &&
//                 (Z < OtherVector.Z + ErrorValue && Z > OtherVector.Z - ErrorValue))
//             {
//                 return true;
//             }

//             return false;
//         }

//         #endregion public override bool Equals(object obj)

//         #endregion Overrides

//         #endregion Public Members

//         #region IEquatable<Vector3> Members

//         /// <summary>Indicates whether the current vector is equal to another vector.</summary>
//         /// <param name="other">A vector to compare with this vector.</param>
//         /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
//         public bool Equals(Vector3d other)
//         {
//             return
//                 X == other.X &&
//                 Y == other.Y &&
//                 Z == other.Z;
//         }

//         #endregion IEquatable<Vector3> Members

//         public static double ComponentMax(Vector3d vector3)
//         {
//             return Math.Max(vector3.X, Math.Max(vector3.Y, vector3.Z));
//         }

//         public static double ComponentMin(Vector3d vector3)
//         {
//             return Math.Min(vector3.X, Math.Min(vector3.Y, vector3.Z));
//         }
//     }
// }