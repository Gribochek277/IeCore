using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreEntites.Materials;
using IeCoreEntites.Model;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IeCore.DefaultImplementations.Primitives
{
    public class Triangle 
    {
        private Model _triangleDefaultModel { get; }
        public SceneObject TriangleSceneObject { get; }

        public Triangle()
        {
            _triangleDefaultModel = new Model("Triangle", "defaultTriangleFile");
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint>() { 0, 1, 2 };

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); //Bottom-left vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.0f, 0.5f, 0.0f), TextureCoordinates = new Vector2(0.5f, 1.0f) }); //Top vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, 0.0f), TextureCoordinates = new Vector2(0.0f, 0.0f) }); //Bottom-right vertex
          
            mesh.Vertices = vertices;
            mesh.Elements = elements;

            _triangleDefaultModel.Meshes.Add(mesh);

            Context.Assetmanager.Register(_triangleDefaultModel);
            ModelComponent modelSceneObject = new ModelComponent(_triangleDefaultModel.Name);
            MaterialComponent materiaComponent = new MaterialComponent();
            Material material = new Material("TriangleDiffuse", "reactangleFile");
            Context.Assetmanager.Register(material);
            material.DiffuseColor = new Vector4(2, 0, 0, 1);
            material.DiffuseTexture = Context.Assetmanager.Retrieve<Texture>("CheckerboardTexture_resolution_2048x2048");
            materiaComponent.materials.Add(material.Name, material); //TODO: Find out how to force user to use asset manager
            TriangleSceneObject = new SceneObject(IrrationalEngine.ServiceProvider.GetService<ILogger<SceneObject>>());
            TriangleSceneObject.AddComponent(modelSceneObject);
            TriangleSceneObject.AddComponent(materiaComponent);
        }
    }
}
