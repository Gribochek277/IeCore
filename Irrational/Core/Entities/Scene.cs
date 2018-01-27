using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Logic;
using System.Collections.Generic;

namespace Irrational
{
    public class Scene : IScene
    {
        protected List<SceneObject> _sceneObjects = new List<SceneObject>();
        private PlayerCamera _camera; 
        public List<SceneObject> Objects { get { return _sceneObjects; } }
        public SceneObject Camera { get { return _camera; } }

        public virtual void OnLoad()
        {
            _camera = new PlayerCamera();
            _camera.AddComponent(new Camera());            
        }

        public virtual void OnRendered()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnResized()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnUnload()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdated()
        {
            _camera.OnUpdated();
        }
    }
}
