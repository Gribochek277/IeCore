using System.Numerics;

namespace IeCore.Extentions
{
    public static class AssimpExtentions
    {
        public static Matrix4x4 ToNumericMatrix(this Assimp.Matrix4x4 input)
        {
            return new Matrix4x4(input.A1, input.B1, input.C1, input.D1,
                                       input.A2, input.B2, input.C2, input.D2,
                                       input.A3, input.B3, input.C3, input.D3,
                                       input.A4, input.B4, input.C4, input.D4);
        }

        public static Vector3 ToVector3(this Assimp.Vector3D input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }

        public static Quaternion ToQuaternion(this Assimp.Quaternion input)
        {
            return new Quaternion(input.X, input.Y, input.Z, input.W);
        }
    }
}
