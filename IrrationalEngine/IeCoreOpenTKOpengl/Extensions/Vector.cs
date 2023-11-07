using OpenTK.Mathematics;

namespace IeCoreOpenTKOpengl.Extensions
{
	public static class Vector
	{
		public static Vector4 ConvertToOpenTkVector(this System.Numerics.Vector4 vector4)
		{
			return new(vector4.X, vector4.Y, vector4.Z, vector4.W);
		}
	}
}
