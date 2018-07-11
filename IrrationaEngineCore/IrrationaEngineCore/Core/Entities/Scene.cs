using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Logic;
using System.Collections.Generic;

namespace Irrational
{
    public class Scene : IScene
    {
        protected List<SceneObject> _sceneObjects = new List<SceneObject>();
        PlayerCamera _camera;
        protected Skybox _skybox;
        public List<SceneObject> Objects { get { return _sceneObjects; } }
        public SceneObject Camera { get { return _camera; } }

        public virtual void OnLoad()
        {
            _camera = new PlayerCamera();
            if(_skybox == null)
            _skybox = new Skybox();
            _camera.AddComponent(new Camera());
            _sceneObjects.Add(_camera);
            _sceneObjects.Add(_skybox);
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

        public virtual void OnUpdated()
        {
            _camera.OnUpdated();
        }
    }
}
