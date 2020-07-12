using IeCoreEntites.Model;
using System.Collections.Generic;
using System.Numerics;

namespace IeCore.DefaultImplementations.ModelPrimitives
{
    public class Triangle 
    {
        public Model TriangleDefaultModel { get; }

        public Triangle()
        {
            TriangleDefaultModel = new Model("Triangle", "defaultTriangleFile");
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();


            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.0f) }); //Bottom-left vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.0f, 0.5f, 0.0f) }); //Top vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, 0.0f) }); //Bottom-right vertex
          
            mesh.Vertices = vertices;

            TriangleDefaultModel.Meshes.Add(mesh);
        }
    }
}
