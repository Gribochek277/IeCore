using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using System.Collections.Generic;

namespace IrrationalEngineCore.Core.CoreManager
{
    public class SceneManager: ISceneManager
    {
        private List<ISceneObject> _sceneObjects = new List<ISceneObject>();
        public IScene Scene { get; }

        public List<ISceneObject> SceneObjects {get{return _sceneObjects;}}

        public SceneManager(IScene scene)
        {
            Scene = scene;
        }        

        public void OnLoad()
        {
            Scene.OnLoad();
            foreach (SceneObject sceneObject in Scene.SceneObjects)// TODO: i don't like this part of code
                _sceneObjects.Add(sceneObject);
        }

        public void OnRendered()
        {
            //_scene.OnRendered(); TODO: implement
        }

        public void OnResized()
        {
           // _scene.OnResized(); TODO: implement
        }

        public void OnUnload()
        {
           // _scene.OnUnload(); TODO: implement
        }

        public void OnUpdated()
        {
            Scene.OnUpdated();
        }
    }
}
