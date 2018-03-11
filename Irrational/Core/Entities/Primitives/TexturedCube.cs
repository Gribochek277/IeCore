using OpenTK;
using System.Linq;

namespace Irrational
{
    /// <summary>
    /// A cube with texture coordinates giving each side the entire texture
    /// </summary>
    class TexturedCube : Cube
    {
        public TexturedCube()
            : base()
        {
            VertCount = 18;
            IndiceCount = 36;
            TextureCoordsCount = 18;
        }

        public override Vector3[] GetVerts()
        {
            return new Vector3[] {
                //left
                new Vector3(-1f, -1f,  -1f),
                new Vector3(1f, 1f,  -1f),
                new Vector3(1f, -1f,  -1f),
                new Vector3(-1f, 1f,  -1f),

                //back
                new Vector3(1f, -1f,  -1f),
                new Vector3(1f, 1f,  -1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(1f, -1f,  1f),

                //right
                new Vector3(-1f, -1f,  1f),
                new Vector3(1f, -1f,  1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, 1f,  1f),

                //top
                new Vector3(1f, 1f,  -1f),
                new Vector3(-1f, 1f,  -1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, 1f,  1f),

                //front
                new Vector3(-1f, -1f,  -1f),
                new Vector3(-1f, 1f,  1f),
                new Vector3(-1f, 1f,  -1f),
                new Vector3(-1f, -1f,  1f),

                //bottom
                new Vector3(-1f, -1f,  -1f),
                new Vector3(1f, -1f,  -1f),
                new Vector3(1f, -1f,  1f),
                new Vector3(-1f, -1f,  1f)

            };
        }

        public override int[] GetIndices(int offset = 0)
        {
           return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        public override Vector2[] GetTextureCoords()
        {
            return new Vector2[] {
                // left
                new Vector2(0.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
 
                // back
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
 
                // right
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
 
                // top
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),
 
                // front
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
 
                // bottom
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f)
            };
        }
    }
}
