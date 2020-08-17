﻿using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreEntites.Materials;
using IeCoreEntites.Model;
using System.Collections.Generic;
using System.Numerics;

namespace IeCore.DefaultImplementations.Primitives
{
    public class Rectangle
    {
        private Model _rectangleDefaultModel;
        public SceneObject RectangleSceneObject { get; }

        public Rectangle()
        {
            _rectangleDefaultModel = new Model("Rectangle", "defaultRectangleFile");
            Mesh mesh = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> elements = new List<uint>() 
            { 
                0, 1, 3, 
                1, 2, 3
            };

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, 0.5f, 0.0f) }); //Top-right vertex

            vertices.Add(new Vertex() { Position = new Vector3(0.5f, -0.5f, 0.0f) }); //Bottom-right vertex

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, -0.5f, 0.0f) }); //Bottom vertex

            vertices.Add(new Vertex() { Position = new Vector3(-0.5f, 0.5f, 0.0f) }); //Top-left vertex

            mesh.Vertices = vertices;
            mesh.Elements = elements;

            _rectangleDefaultModel.Meshes.Add(mesh);
            
            Context.Assetmanager.Register(_rectangleDefaultModel);
            ModelComponent modelSceneObject = new ModelComponent(_rectangleDefaultModel.Name);
            MaterialComponent materiaComponent = new MaterialComponent();
            Material material = new Material("RectangleDiffuse", "reactangleFile");
            Context.Assetmanager.Register(material);
            material.DiffuseColor = new Vector4(0, 2, 0, 1);
            materiaComponent.materials.Add(material.Name, material);
            RectangleSceneObject = new SceneObject();
            RectangleSceneObject.AddComponent(modelSceneObject);
            RectangleSceneObject.AddComponent(materiaComponent);
        }
    }
}