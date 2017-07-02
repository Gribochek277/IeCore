using System;
using OpenTK;
using System.Collections.Generic;

namespace IrrationalSpace
{
    public class Mesh : Volume
    {
        public Vector3[] vertices;
        public Vector3[] colors;
        public Vector2[] texturecoords;

        public List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

        public override int VertCount { get { return vertices.Length; } }
        public override int IndiceCount { get { return faces.Count * 3; } }
        public override int ColorDataCount { get { return colors.Length; } }

        /// <summary>
        /// Get vertices for this object
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetVerts()
        {
            return vertices;
        }

        /// <summary>
        /// Get indices to draw this object
        /// </summary>
        /// <param name="offset">Number of vertices buffered before this object</param>
        /// <returns>Array of indices with offset applied</returns>
        public override int[] GetIndices(int offset = 0)
        {
            List<int> temp = new List<int>();

            foreach (var face in faces)
            {
                temp.Add(face.Item1 + offset);
                temp.Add(face.Item2 + offset);
                temp.Add(face.Item3 + offset);
            }

            return temp.ToArray();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetColorData()
        {
            return colors;
        }

        /// <summary>
        /// Get texture coordinates
        /// </summary>
        /// <returns></returns>
        public override Vector2[] GetTextureCoords()
        {
            return texturecoords;
        }


        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.Scale(Scale) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) * Matrix4.CreateTranslation(Position);
        }

    }
}

