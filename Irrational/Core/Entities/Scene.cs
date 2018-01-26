using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Logic;
using System.Collections.Generic;

namespace Irrational
{
    public class Scene : IScene
    {
        private List<SceneObject> _sceneObjects = new List<SceneObject>();
        private PlayerCamera _camera; 
        public List<SceneObject> Objects { get { return _sceneObjects; } }
        public SceneObject Camera { get { return _camera; } }

        public void OnLoad()
        {
            _camera = new PlayerCamera();
            _camera.AddComponent(new Camera());

            Lion gameObject = new Lion();
            Knight knight = new Knight();
            _sceneObjects.Add(knight);
            _sceneObjects.Add(gameObject);
            
        }

        public void OnRendered()
        {
            throw new System.NotImplementedException();
        }

        public void OnResized()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdated()
        {
            _camera.OnUpdated();
        }
    }
}
