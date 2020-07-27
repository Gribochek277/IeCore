using System.Collections.Generic;
using IrrationalEngineCore.Logic;
using IrrationalEngineCore.Core.Entities.Abstractions;

namespace IrrationalEngineCore.Core.Entities
{
    public class Scene : IScene {
        protected List<ISceneObject> _sceneObjects = new List<ISceneObject> ();
        private PlayerCamera _camera;
        protected Skybox _skybox;
        public List<ISceneObject> SceneObjects { get { return _sceneObjects; } }
        public SceneObject Camera { get { return _camera; } }

        public virtual void OnLoad () {
            _camera = new PlayerCamera ();
            if (_skybox == null)
                _skybox = new Skybox ();
            _camera.AddComponent (new Camera ());
            _sceneObjects.Add (_camera);
            _sceneObjects.Add (_skybox);
        }

        public virtual void OnRendered () {
            throw new System.NotImplementedException ();
        }

        public virtual void OnResized () {
            throw new System.NotImplementedException ();
        }

        public virtual void OnUnload () {
            throw new System.NotImplementedException ();
        }

        public virtual void OnUpdated () {
            _camera.OnUpdated ();
        }
    }
}