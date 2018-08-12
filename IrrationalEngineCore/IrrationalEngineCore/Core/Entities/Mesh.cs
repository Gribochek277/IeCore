using OpenTK;
using System.Linq;

namespace Irrational.Core.Entities
{
    public class Mesh
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;
        public int[] Indeces { get; set; }

        public Vector3[] Vertices { get; set; }

        public Vector3[] Normals { get; set; }

        public Vector2[] UvCoords { get; set; }
        
        public int IndiceCount { get { return Indeces.Length; } }

        public int ColorDataCount { get { return Indeces.Length; } }

        public int VertCount { get { return Vertices.Length; } }

        public int NormalCount { get { return Normals.Length; } }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns>Vertecies forobject</returns>
        public Vector3[] GetVerts()
        {          
                return Vertices.ToArray();
        }

        /// <summary>
        /// Get normals for this object
        /// </summary>
        /// <returns>normals for object</returns>
        public Vector3[] GetNormals()
        {
                return Normals.ToArray();
        }

        /// <summary>
        /// Get indices to draw this object
        /// </summary>
        /// <param name="offset">Number of vertices buffered before this object</param>
        /// <returns>Array of indices with offset applied</returns>
        public int[] GetIndices(int offset = 0)
        {
            return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public Vector3[] GetColorData()
        {
            return new Vector3[ColorDataCount];
        }

        /// <summary>
        /// Get texture coordinates
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetTextureCoords()
        {           
                return UvCoords.ToArray();
        }


        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

        /// <summary>
        /// Calculates the model normals
        /// </summary>
        public void CalculateNormals()
        {
            Vector3[] normals = new Vector3[VertCount];
            Vector3[] verts = GetVerts();
            int[] inds = GetIndices();

            // Compute normals for each face
            for (int i = 0; i < IndiceCount; i += 3)
            {
                Vector3 v1 = verts[inds[i]];
                Vector3 v2 = verts[inds[i + 1]];
                Vector3 v3 = verts[inds[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < NormalCount; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            Normals = normals;
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

