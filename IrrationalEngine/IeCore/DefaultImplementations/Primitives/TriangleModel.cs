using System.Collections.Generic;
using System.Numerics;
using IeCoreEntities.Model;

namespace IeCore.DefaultImplementations.Primitives
{
    public class TriangleModel: Model
    {

        public TriangleModel(string name, string file) : base(name, file)
        {
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint> { 0, 1, 2 };

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); //Bottom-left vertex

            vertices.Add(new Vertex { Position = new Vector3(0.0f, 0.5f, 0.0f), TextureCoordinates = new Vector2(0.5f, 1.0f) }); //Top vertex

            vertices.Add(new Vertex { Position = new Vector3(0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(0.0f, 0.0f) }); //Bottom-right vertex
          
            mesh.Vertices = vertices;
            mesh.Elements = elements;

            Meshes.Add(mesh);
        }
    }
}
