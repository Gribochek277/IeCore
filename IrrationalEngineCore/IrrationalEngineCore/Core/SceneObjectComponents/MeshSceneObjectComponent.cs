using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Utils.Interfaces;
using System;

namespace Irrational.Core.SceneObjectComponents
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
