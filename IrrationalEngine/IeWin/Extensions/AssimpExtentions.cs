using System.Numerics;
using Assimp;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Quaternion = System.Numerics.Quaternion;

namespace IeWin.Extensions
{
	/// <summary>
	/// Assim extension
	/// </summary>
	public static class AssimpExtentions
	{
		/// <summary>
		/// Convert of matrxi 4x4
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Matrix4x4 ToNumericMatrix(this Assimp.Matrix4x4 input)
		{
			return new Matrix4x4(input.A1, input.B1, input.C1, input.D1,
									   input.A2, input.B2, input.C2, input.D2,
									   input.A3, input.B3, input.C3, input.D3,
									   input.A4, input.B4, input.C4, input.D4);
		}

		/// <summary>
		/// Convert of vector3
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Vector3 ToVector3(this Vector3D input)
		{
			return new Vector3(input.X, input.Y, input.Z);
		}

		/// <summary>
		/// Convert to quaternion
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static Quaternion ToQuaternion(this Assimp.Quaternion input)
		{
			return new Quaternion(input.X, input.Y, input.Z, input.W);
		}
	}
}
