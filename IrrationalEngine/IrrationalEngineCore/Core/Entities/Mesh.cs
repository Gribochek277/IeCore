using System.Linq;
using OpenTK;

namespace IrrationalEngineCore.Core.Entities {
    public class Mesh {
        private bool _recalculateNormals = false;
        
        public Transform Transform {get; set;}
        public int[] Indeces { get; set; }

        public Vector3[] Vertices { get; set; }

        public Vector3[] Normals { get; set; }

        public Vector2[] UvCoords { get; set; }

        public int IndiceCount { get { return Indeces.Length; } }

        public int ColorDataCount { get { return Indeces.Length; } }

        public int VertCount { get { return Vertices.Length; } }

        public int NormalCount { get { return Normals.Length; } }

        public Mesh(bool recalculateNormals = false){
            Transform = new Transform();
            _recalculateNormals = recalculateNormals;
        }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns>Vertecies forobject</returns>
        public Vector3[] GetVerts () {
            return Vertices.ToArray ();
        }

        /// <summary>
        /// Get normals for this object
        /// </summary>
        /// <returns>normals for object</returns>
        public Vector3[] GetNormals () {

            if(_recalculateNormals) CalculateNormals();
            return Normals.ToArray ();
        }

        /// <summary>
        /// Get indices to draw this object
        /// </summary>
        /// <param name="offset">Number of vertices buffered before this object</param>
        /// <returns>Array of indices with offset applied</returns>
        public int[] GetIndices (int offset = 0) {
            return Enumerable.Range (offset, IndiceCount).ToArray ();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public Vector3[] GetColorData () {
            return new Vector3[ColorDataCount];
        }

        /// <summary>
        /// Get texture coordinates
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetTextureCoords () {
            return UvCoords;
        }

       

        /// <summary>
        /// Calculates the model normals
        /// </summary>
        public void CalculateNormals() {
            Vector3[] normals = new Vector3[VertCount];
            Vector3[] verts = GetVerts ();
            int[] inds = GetIndices ();

            // Compute normals for each face
            for (int i = 0; i < IndiceCount; i += 3) {
                Vector3 v1 = verts[inds[i]];
                Vector3 v2 = verts[inds[i + 1]];
                Vector3 v3 = verts[inds[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross (v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross (v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross (v2 - v1, v3 - v1);
            }

            for (int i = 0; i < NormalCount; i++) {
                normals[i] = normals[i].Normalized();
            }

            Normals = normals;
        }

    }

    //Helper class for more readable realisation
    public class FaceVertex {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoord;

        public FaceVertex (Vector3 pos, Vector3 norm, Vector2 texcoord) {
            Position = pos;
            Normal = norm;
            TextureCoord = texcoord;
        }
    }
}