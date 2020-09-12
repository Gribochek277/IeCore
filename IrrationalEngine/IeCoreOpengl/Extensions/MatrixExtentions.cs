namespace IeCoreOpengl.Extensions
{
    public static class MatrixExtentions
    {
        public static OpenTK.Mathematics.Matrix4 ConvertToOpenTKMaxtrix4(this System.Numerics.Matrix4x4 matrix4)
        {
            return new OpenTK.Mathematics.Matrix4(
                matrix4.M11, matrix4.M12, matrix4.M13, matrix4.M14,
                matrix4.M21, matrix4.M22, matrix4.M23, matrix4.M24,
                matrix4.M31, matrix4.M32, matrix4.M33, matrix4.M34,
                matrix4.M41, matrix4.M42, matrix4.M43, matrix4.M44);
        }
    }
}
