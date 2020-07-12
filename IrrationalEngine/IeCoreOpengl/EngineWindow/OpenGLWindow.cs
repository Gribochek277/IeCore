using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace IeCoreOpengl.EngineWindow
{
    public class OpenGLWindow : IWindow
    {
        private GameWindow _gameWindow; //TODO: Wrap this class with own class
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public static Rectangle Bounds;

        public event EventHandler LoadingComplete;

        public double Time { get; set; } = 0.0d;
        public int UpdateRate { private get; set; } = 30;
        public int FrameRate { private get; set; } = 60;

        public OpenGLWindow(int resX, int resY, string title, IRenderer renderer, ISceneManager sceneManager)
        {
            _gameWindow = new GameWindow(resX, resY, GraphicsMode.Default, title, GameWindowFlags.Default, 
                DisplayDevice.Default, 3,3, GraphicsContextFlags.Default);
         
            _renderer = renderer;
            _sceneManager = sceneManager;
            AddListeners();
        }

        private void AddListeners()
        {
            _gameWindow.Load += (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload += (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame += (object o, FrameEventArgs e) => { OnRender(); };
            _gameWindow.UpdateFrame += (object o, FrameEventArgs e) => { OnUpdated(); };
            _gameWindow.Resize += (object o, EventArgs e) => { OnResized(); };
        }

        private void RemoveListeners()
        {
            _gameWindow.Load -= (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload -= (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame -= (object o, FrameEventArgs e) => { OnRender(); };
            _gameWindow.UpdateFrame -= (object o, FrameEventArgs e) => { OnUpdated(); };
            _gameWindow.Resize -= (object o, EventArgs e) => { OnResized(); };
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


        public void OnRender()
        {
            _sceneManager.OnRender();
            _renderer.OnRender();
            _gameWindow.SwapBuffers();
            Time += _gameWindow.RenderPeriod;
            _gameWindow.Title = _sceneManager.Scene.GetType().Name + " FPS: " + (1d / _gameWindow.RenderPeriod).ToString("0.");           
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

        public void Run()
        {
            _gameWindow.Run(UpdateRate, FrameRate);
        }

        public void OnUnload()
        {
            RemoveListeners();
            _gameWindow.Dispose();
        }
    }
}
