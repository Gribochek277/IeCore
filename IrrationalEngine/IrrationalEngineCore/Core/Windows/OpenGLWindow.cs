using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Windows.Abstractions;
using OpenTK;
using System;
using IrrationalEngineCore.Core.Renderer.Abstractions;

namespace IrrationalEngineCore.Core.Windows
{
    public class OpenGLWindow : IWindow
    {
        private GameWindow _gameWindow; //TODO: Wrap this class with own class
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public static Rectangle Bounds;

        public event EventHandler LoadingComplete;

        public double Time { get; set; } = 0.0d;

        public OpenGLWindow(GameWindow gameWindow, IRenderer renderer, ISceneManager sceneManager)
        {
            _gameWindow =gameWindow;
            _renderer = renderer;
            _sceneManager = sceneManager;
            AddListeners();            
        }
        public void Run()
        {
            _gameWindow.Run(60, 60);
        }

        private void AddListeners()
        {
            _gameWindow.Load += (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload += (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame += (object o, FrameEventArgs e) => { OnRendered(); };
            _gameWindow.UpdateFrame += (object o, FrameEventArgs e) => { OnUpdated(); };
            _gameWindow.Resize += (object o, EventArgs e) => { OnResized(); };
        }

        public void OnUnload()
        {
            RemoveListeners();
            _gameWindow.Dispose();
        }

        public void RemoveListeners()
        {
            _gameWindow.Load -= (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload -= (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame -= (object o, FrameEventArgs e) => { OnRendered(); };
            _gameWindow.UpdateFrame -= (object o, FrameEventArgs e) => { OnUpdated(); };
            _gameWindow.Resize -= (object o, EventArgs e) => { OnResized(); };
            //TODO: Check memory leak after closing window
        }

        public void OnLoad()
        {
            Bounds = _gameWindow.Bounds;
            _sceneManager.OnLoad();
            _renderer.OnLoad();
            _renderer.SetViewPort(_gameWindow.ClientSize.Width, _gameWindow.ClientSize.Height);
            _gameWindow.Title = _sceneManager.Scene.GetType().Name;
            LoadingComplete?.Invoke(this, null);
        }
        

        public void OnRendered()
        {
            _sceneManager.OnRendered();
            _renderer.OnRendered();            
            Time += _gameWindow.RenderPeriod;
            _gameWindow.Title = _sceneManager.Scene.GetType().Name + " FPS: " + (1d / _gameWindow.RenderPeriod).ToString("0.");

            _gameWindow.SwapBuffers();
        }

        public void OnResized()
        {
            Bounds = _gameWindow.Bounds;
            _sceneManager.OnResized();
            _renderer.OnResized();
            _renderer.SetViewPort(_gameWindow.ClientSize.Width, _gameWindow.ClientSize.Height);
        }

        public void OnUpdated()
        {
            _sceneManager.OnUpdated();
            _renderer.OnUpdated();
        }
    }
}
