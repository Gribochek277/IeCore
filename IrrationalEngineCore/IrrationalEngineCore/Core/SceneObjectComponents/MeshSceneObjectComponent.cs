using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Utils.Interfaces;
using System;

namespace Irrational.Core.SceneObjectComponents
{
    public class MeshSceneObjectComponent : ISceneObjectComponent
    {

        private Mesh _modelMesh;
        private string _mdlSource;
        private IModelLoader _modelLoader;

        public MeshSceneObjectComponent() { }

        public MeshSceneObjectComponent(IModelLoader modelLoader, string modelSource)
        {
            _modelLoader = modelLoader;
            _mdlSource = modelSource;
        }

        public Mesh ModelMesh
        {
            get { return _modelMesh; }
            set { _modelMesh = value; }
        }

        public void OnLoad()
        {
            if(_modelLoader!=null)
            _modelMesh = _modelLoader.LoadFromFile(_mdlSource);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
