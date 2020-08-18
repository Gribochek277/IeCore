using IeCore.DefaultImplementations.Primitives;
using IeCore.DefaultImplementations.SceneObjects;
using IeCoreInterfaces.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace IeCore.DefaultImplementations.Scene
{
    public class DefaultScene : IScene
    {
        private ILogger<DefaultScene> _logger;
        public DefaultScene(ILogger<DefaultScene> logger)
        {
            _logger = logger;
        }
        protected List<ISceneObject> _sceneObjects = new List<ISceneObject>();
        //private PlayerCamera _camera;
        //protected Skybox _skybox;
        public IEnumerable<ISceneObject> SceneObjects { get { return _sceneObjects; } }
        // public SceneObject Camera { get { return _camera; } }
        public virtual void OnLoad()
        {     
            for(int i=0; i<1; i++)
            { 
                _sceneObjects.Add(new Rectangle().RectangleSceneObject);
                _sceneObjects[i].Name = "Scene object #" + i;
                _logger.LogInformation(i.ToString());
            }

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
