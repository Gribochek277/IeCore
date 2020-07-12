using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Loaders.Interfaces;
using OpenTK;
using System;

namespace IrrationalEngineCore.Core.SceneObjectComponents
{
    public class MeshSceneObjectComponent : ISceneObjectComponent
    {
        public Mesh ModelMesh { get; private set; }
        public string MdlSource { get; set; }
        public IModelLoader ModelLoader { get; private set; }

        public Vector3 Position { get { return ModelMesh.Transform.Position; } }

        public Vector3 Rosition { get { return ModelMesh.Transform.Rotation; } }

        public MeshSceneObjectComponent() { }

        public MeshSceneObjectComponent(IModelLoader modelLoader, string modelSource)
        {
            ModelLoader = modelLoader;
            MdlSource = modelSource;
        }


        public void OnLoad()
        {
            if(ModelLoader!=null)
            ModelMesh = ModelLoader.LoadFromFile(MdlSource);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
