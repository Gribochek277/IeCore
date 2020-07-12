using IeCore.DefaultImplementations.Scene;
using IeCoreInterfaces;
using IeCoreInterfaces.Core;
using System;

namespace IeCore
{
    public class SceneManager : ISceneManager
    {
        private IScene _defaultScene;
        public IScene Scene { get; private set; }

        public SceneManager()
        {
            _defaultScene = new DefaultScene();
            Scene = _defaultScene;
        }

        public void OnLoad()
        {           
            Scene.OnLoad();
        }

        public void OnRender()
        {
            //throw new NotImplementedException();
        }

        public void OnResized()
        {
                //throw new NotImplementedException();
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        public void OnUpdated()
        {
            Scene.OnUpdated();
        }
    }
}
