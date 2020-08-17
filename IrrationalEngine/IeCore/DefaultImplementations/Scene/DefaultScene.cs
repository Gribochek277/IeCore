using IeCore.DefaultImplementations.Primitives;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreInterfaces.Core;
using System.Collections.Generic;

namespace IeCore.DefaultImplementations.Scene
{
    public class DefaultScene : IScene
    {
        protected List<ISceneObject> _sceneObjects = new List<ISceneObject>();
        //private PlayerCamera _camera;
        //protected Skybox _skybox;
        public IEnumerable<ISceneObject> SceneObjects { get { return _sceneObjects; } }
        // public SceneObject Camera { get { return _camera; } }
        public virtual void OnLoad()
        {           
            _sceneObjects.Add(new Rectangle().RectangleSceneObject);
            _sceneObjects.Add(new Triangle().TriangleSceneObject);

            foreach (SceneObject _sceneObject in _sceneObjects)
            {
                _sceneObject.OnLoad();
            }
        }

        public void OnRender()
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
            //_camera.OnUpdated();
        }
    }
}
