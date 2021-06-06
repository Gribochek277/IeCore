using System.Collections.Generic;
using System.Numerics;
using IeCoreEntities.Model;

namespace IeCore.DefaultImplementations.Primitives
{
    public class CubeModel : Model
    {
        public CubeModel(string name, string file) : base(name, file)
        {
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint>
            {
                0, 1, 3,
                1, 2, 3,

                4, 5, 7,
                5, 6, 7,

                0, 1, 4,
                1, 4, 5,

                2, 3, 6,
                3, 7, 6,

                5, 1, 6,
                6, 2, 1,

                3, 7, 4,
                4, 0, 3
            };

            vertices.Add(new Vertex { Position = new Vector3(0.5f, 0.5f, 0.5f), TextureCoordinates = new Vector2(1.0f, 1.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(0.5f, -0.5f, 0.5f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, -0.5f, 0.5f), TextureCoordinates = new Vector2(0.0f, 0.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, 0.5f, 0.5f), TextureCoordinates = new Vector2(0.0f, 1.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(0.5f, 0.5f, -0.5f), TextureCoordinates = new Vector2(1.0f, 1.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(0.5f, -0.5f, -0.5f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); 

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), TextureCoordinates = new Vector2(0.0f, 0.0f) });

            vertices.Add(new Vertex { Position = new Vector3(-0.5f, 0.5f, -0.5f), TextureCoordinates = new Vector2(0.0f, 1.0f) }); 

            mesh.Vertices = vertices;
            mesh.Elements = elements;

            Meshes.Add(mesh);
        }
    }
}
