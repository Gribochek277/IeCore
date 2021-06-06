using System.Collections.Generic;
using System.Numerics;
using IeCoreEntities.Model;

namespace IeCore.DefaultImplementations.Primitives
{
    public class RectangleModel: Model
    {
        public RectangleModel(string name, string file) : base(name, file)
        {
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint>
            {
                0, 1, 3,
                1, 2, 3
            };

            vertices.Add(new Vertex { Position = new Vector3(0.5f, 0.5f, 0.0f), TextureCoordinates = new Vector2(1.0f, 1.0f) }); //Top-right vertex

            vertices.Add(new Vertex { Position = new Vector3(0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); //Bottom-right vertex

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(0.0f, 0.0f) }); //Bottom vertex

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, 0.5f, 0.0f), TextureCoordinates = new Vector2(0.0f, 1.0f) }); //Top-left vertex

            mesh.Vertices = vertices;
            mesh.Elements = elements;

            Meshes.Add(mesh);
        }
    }
}
