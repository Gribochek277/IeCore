using System.Numerics;
using Silk.NET.Maths;

namespace IeCoreSilkNetOpenGl.Extensions;

public static class Matrix
{
	public static Matrix4X4<float> ToMatrix4X4(this Matrix4x4 matrix4)
	{
		return new Matrix4X4<float>(
			matrix4.M11, matrix4.M12, matrix4.M13, matrix4.M14,
			matrix4.M21, matrix4.M22, matrix4.M23, matrix4.M24,
			matrix4.M31, matrix4.M32, matrix4.M33, matrix4.M34,
			matrix4.M41, matrix4.M42, matrix4.M43, matrix4.M44);
	}
	
	public static ReadOnlySpan<float> ToReadOnlySpan(this Matrix4x4 matrix4)
	{
		float[] array = {
			matrix4.M11, matrix4.M12, matrix4.M13, matrix4.M14,
			matrix4.M21, matrix4.M22, matrix4.M23, matrix4.M24,
			matrix4.M31, matrix4.M32, matrix4.M33, matrix4.M34,
			matrix4.M41, matrix4.M42, matrix4.M43, matrix4.M44
		};
		return new ReadOnlySpan<float>(array);
	}
}