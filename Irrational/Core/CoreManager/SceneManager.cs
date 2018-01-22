using Irrational.Core.CoreManager.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Renderer.Abstractions;
using System.Collections.Generic;

namespace Irrational.Core.CoreManager
{
    public class SceneManager: ISceneManager
    {
        private IRenderer _renderer;
        private List<ISceneObject> _sceneObjects = new List<ISceneObject>();
        private Scene _scene = new Scene();
        public SceneManager(IRenderer renderer)
        {
            _renderer = renderer;
        }        

        public void OnLoad()
        {
            _scene.OnLoad();
            foreach (SceneObject sceneObject in _scene.Objects)
                _sceneObjects.Add(sceneObject);
            _renderer.OnLoad(_sceneObjects);
        }

        public void OnRendered()
        {
            _renderer.OnRendered();
        }

        public void OnResized()
        {
            _renderer.OnResized();
        }

        public void OnUnload()
        {
            _renderer.OnUnload();
        }

        public void OnUpdated()
        {
            _renderer.OnUpdated();
        }
    }
}
