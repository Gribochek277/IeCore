using Assimp;
using System.Numerics;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Quaternion = System.Numerics.Quaternion;

namespace IeCore.Extensions
{
	public static class AssimpExtensions
	{
		public static Matrix4x4 ToNumericMatrix(this Assimp.Matrix4x4 input)
		{
			return new Matrix4x4(input.A1, input.A2, input.A3, input.A4,
									   input.B1, input.B2, input.B3, input.B4,
									   input.C1, input.C2, input.C3, input.C4,
									   input.D1, input.D2, input.D3, input.D4);
		}

		public static Vector3 ToVector3(this Vector3D input)
		{
			return new Vector3(input.X, input.Y, input.Z);
		}

		public static Quaternion ToQuaternion(this Assimp.Quaternion input)
		{
			return new Quaternion(input.X, input.Y, input.Z, input.W);
		}
	}
}
