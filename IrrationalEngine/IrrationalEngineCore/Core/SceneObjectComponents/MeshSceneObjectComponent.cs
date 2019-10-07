using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Loaders.Interfaces;
using System;

namespace IrrationalEngineCore.Core.SceneObjectComponents
{
    public class MeshSceneObjectComponent : ISceneObjectComponent
    {
        public Mesh ModelMesh { get; private set; }
        public string MdlSource { get; set; }
        public IModelLoader ModelLoader { get; private set; }

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
