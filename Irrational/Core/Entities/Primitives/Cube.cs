using OpenTK;
using System.Linq;

namespace Irrational
{
    /// <summary>
    /// Cube
    /// </summary>
    public class Cube : Volume
    {
        public Cube()
        {
            VertCount = 8;
            IndiceCount = 36;
            ColorDataCount = 8;
        }

        public override Vector3[] GetVerts()
        {
            return new Vector3[] {new Vector3(-1f, -1f,  -1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(1f, -1f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(-1f, 1f, 1f),
            };
        }

        public override int[] GetIndices(int offset = 0)
        {
           return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        public override Vector3[] GetColorData()
        {
            return new Vector3[] {
                new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 1f, 0f),
                new Vector3( 1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 1f, 0f),
                new Vector3( 1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f)
            };
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        public override Vector2[] GetTextureCoords()
        {
            return new Vector2[] { };
        }
    }
}
