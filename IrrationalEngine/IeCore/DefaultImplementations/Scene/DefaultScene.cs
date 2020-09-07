using IeCore.DefaultImplementations.Primitives;
using IeCore.DefaultImplementations.SceneObjects;
using IeCore.DefaultImplementations.Textures;
using IeCoreInterfaces.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;

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

        public ISceneObject MainCamera { set; private get; }

        // public SceneObject Camera { get { return _camera; } }
        public virtual void OnLoad()
        {
          
            //Generate and register in memory textures
            //TODO: Consider of creating stage for generating and registering all default objects.
            Context.Assetmanager.Register(DeafultTexture.CreateDefaultCheckerboard(2048, 512));
            MainCamera = new SceneObject(IrrationalEngine.ServiceProvider.GetService<ILogger<SceneObject>>());
            MainCamera.AddComponent(new Camera());
            _sceneObjects.Add(MainCamera);
            for (int i=0; i<100; i++)
            { 
                _sceneObjects.Add(new Triangle().TriangleSceneObject);
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
            foreach(var sceneobject in SceneObjects)
            {
                sceneobject.Rotation += new Vector3(0, 0, 0.03f * (float)Context.RendrerDeltaTime);
            }
        }
    }
}
