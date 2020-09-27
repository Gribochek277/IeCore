using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreEntites.Materials;
using IeCoreEntites.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IeCore.DefaultImplementations.Primitives
{
    public class Cube
    {
        private Model _cubeDefaultModel;
        public SceneObject CubeSceneObject { get; }

        public Cube()
        {
            _cubeDefaultModel = new Model(string.Concat("Cube", Guid.NewGuid().ToString()), "defaultCubeFile");
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint>()
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

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, 0.5f, 0.5f), TextureCoordinates = new Vector2(1.0f, 1.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, 0.5f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.5f), TextureCoordinates = new Vector2(0.0f, 0.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, 0.5f, 0.5f), TextureCoordinates = new Vector2(0.0f, 1.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, 0.5f, -0.5f), TextureCoordinates = new Vector2(1.0f, 1.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, -0.5f), TextureCoordinates = new Vector2(1.0f, 0.0f) }); 

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, -0.5f), TextureCoordinates = new Vector2(0.0f, 0.0f) });

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, 0.5f, -0.5f), TextureCoordinates = new Vector2(0.0f, 1.0f) });
            mesh.Vertices = vertices;
            mesh.Elements = elements;

            _cubeDefaultModel.Meshes.Add(mesh);

            Context.Assetmanager.Register(_cubeDefaultModel);
            ModelComponent modelSceneObject = new ModelComponent(_cubeDefaultModel.Name);
            MaterialComponent materiaComponent = new MaterialComponent();
            Material material = new Material("CubeDiffuse", "cubeFile");
            Context.Assetmanager.Register(material);
            material.DiffuseColor = new Vector4(0, 2, 0, 1);
            material.DiffuseTexture = Context.Assetmanager.Retrieve<Texture>("CheckerboardTexture_resolution_2048x2048");
            materiaComponent.materials.Add(material.Name, material);
            CubeSceneObject = new SceneObject(IrrationalEngine.ServiceProvider.GetService<ILogger<SceneObject>>());
            CubeSceneObject.AddComponent(modelSceneObject);
            CubeSceneObject.AddComponent(materiaComponent);
        }
    }
}
