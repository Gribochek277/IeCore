using IeCore.DefaultImplementations.ModelPrimitives;
using IeCore.DefaultImplementations.SceneObjectComponents;
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
            //    _camera = new PlayerCamera();
            //    if (_skybox == null)
            //       _skybox = new Skybox();
            // _camera.AddComponent(new Camera());
            //    _sceneObjects.Add(_camera);
            //   _sceneObjects.Add(_skybox);
            Context.Assetmanager.Register(new Triangle().TriangleDefaultModel);
            ModelComponent modelSceneObject = new ModelComponent("Triangle");
            MaterialComponent materiaComponent = new MaterialComponent();
            SceneObject sceneObject = new SceneObject();
            sceneObject.AddComponent(modelSceneObject);
            sceneObject.AddComponent(materiaComponent);
            _sceneObjects.Add(sceneObject);
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
