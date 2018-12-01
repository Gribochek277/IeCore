using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Utils.Interfaces;
using System;

namespace Irrational.Core.SceneObjectComponents
{
    public class MeshSceneObjectComponent : ISceneObjectComponent
    {
        private string _mdlSource;
        private IModelLoader _modelLoader;

        public Mesh ModelMesh { get; private set; }

        public MeshSceneObjectComponent() { }

        public MeshSceneObjectComponent(IModelLoader modelLoader, string modelSource)
        {
            _modelLoader = modelLoader;
            _mdlSource = modelSource;
        }


        public void OnLoad()
        {
            if(_modelLoader!=null)
            ModelMesh = _modelLoader.LoadFromFile(_mdlSource);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
