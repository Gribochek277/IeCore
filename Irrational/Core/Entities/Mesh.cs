using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace IrrationalSpace
{
    public class Mesh : Volume
    {
        public List<Tuple<FaceVertex, FaceVertex, FaceVertex>> faces = new List<Tuple<FaceVertex, FaceVertex, FaceVertex>>();

        public override int VertCount { get { return faces.Count * 3; } }

        public override int IndiceCount { get { return faces.Count * 3; } }

        public override int ColorDataCount { get { return faces.Count * 3; } }

        public override int TextureCoordsCount { get { return faces.Count * 3; } }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetVerts()
        {
            List<Vector3> verts = new List<Vector3>();

            foreach (var face in faces)
            {
                verts.Add(face.Item1.Position);
                verts.Add(face.Item2.Position);
                verts.Add(face.Item3.Position);
            }

            return verts.ToArray();
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
            List<Vector2> coords = new List<Vector2>();

            foreach (var face in faces)
            {
                coords.Add(face.Item1.TextureCoord);
                coords.Add(face.Item2.TextureCoord);
                coords.Add(face.Item3.TextureCoord);
            }

            return coords.ToArray();
        }


        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Scale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
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

