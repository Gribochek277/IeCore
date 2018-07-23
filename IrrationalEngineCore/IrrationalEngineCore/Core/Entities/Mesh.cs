using OpenTK;
using System.Linq;

namespace Irrational.Core.Entities
{
    public class Mesh : Volume
    {
        public int[] Indeces { get; set; }

        public Vector3[] Vertices { get; set; }

        public Vector3[] Normals { get; set; }

        public Vector2[] UvCoords { get; set; }
        
        public override int IndiceCount { get { return Indeces.Length; } }

        public override int ColorDataCount { get { return Indeces.Length; } }

        public override int VertCount { get { return Vertices.Length; } }

        public override int TextureCoordsCount { get { return UvCoords.Length; } }

        public override int NormalCount { get { return Normals.Length; } }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns>Vertecies forobject</returns>
        public override Vector3[] GetVerts()
        {          
                return Vertices.ToArray();
        }

        /// <summary>
        /// Get normals for this object
        /// </summary>
        /// <returns>normals for object</returns>
        public override Vector3[] GetNormals()
        {
            if (base.GetNormals().Length > 0)
            {
                return base.GetNormals();
            }
                return Normals.ToArray();
        }

        /// <summary>
        /// Get indices to draw this object
        /// </summary>
        /// <param name="offset">Number of vertices buffered before this object</param>
        /// <returns>Array of indices with offset applied</returns>
        public override int[] GetIndices(int offset = 0)
        {
            return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetColorData()
        {
            return new Vector3[ColorDataCount];
        }

        /// <summary>
        /// Get texture coordinates
        /// </summary>
        /// <returns></returns>
        public override Vector2[] GetTextureCoords()
        {           
                return UvCoords.ToArray();
        }


        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

    }

    //Helper class for more readable realisation
    public class FaceVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoord;

        public FaceVertex(Vector3 pos, Vector3 norm, Vector2 texcoord)
        {
            Position = pos;
            Normal = norm;
            TextureCoord = texcoord;
        }
    }
}

