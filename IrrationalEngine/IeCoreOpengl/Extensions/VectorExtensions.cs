namespace IeCoreOpengl.Extensions
{
    public static class VectorExtensions
    {
        public static OpenToolkit.Mathematics.Vector4 ConvertToOpenTKVector(this System.Numerics.Vector4 vector4)
        {
            return new OpenToolkit.Mathematics.Vector4(vector4.X, vector4.Y, vector4.Z, vector4.W);
        }
    }
}
