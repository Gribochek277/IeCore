using IeCoreEntites.Model;
using System.Collections.Generic;
using System.Numerics;

namespace IeCore.DefaultImplementations.ModelPrimitives
{
    public class Rectangle
    {
        public Model TriangleDefaultModel { get; }

        public Rectangle()
        {
            TriangleDefaultModel = new Model("Rectangle", "defaultRectangleFile");
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<int> elements = new List<int>() { 0, 1, 3, 1, 2, 3};

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.0f) }); //Top-right vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, 0.0f) }); //Bottom-right vertex

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.0f) }); //Bottom vertex

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, 0.5f, 0.0f) }); //Top-left vertex

            mesh.Vertices = vertices;
            mesh.Elements = elements;

            TriangleDefaultModel.Meshes.Add(mesh);
        }
    }
}
