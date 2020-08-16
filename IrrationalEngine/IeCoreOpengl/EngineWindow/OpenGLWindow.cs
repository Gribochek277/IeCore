using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using OpenToolkit.Windowing.Desktop;
using System;
using OpenToolkit.Windowing.Common;

namespace IeCoreOpengl.EngineWindow
{
    public class OpenGLWindow : IWindow
    {
        private GameWindow _gameWindow; //TODO: Wrap this class with own class
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public event EventHandler LoadingComplete;

        public double Time { get; set; } = 0.0d;
        public int UpdateRate { private get; set; } = 30;
        public int FrameRate { private get; set; } = 60;

        public OpenGLWindow(int resX, int resY, string title, IRenderer renderer, ISceneManager sceneManager)
        {
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.RenderFrequency = FrameRate;
            gameWindowSettings.UpdateFrequency = UpdateRate;
            NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.APIVersion = new Version(3, 2);
            nativeWindowSettings.Size = new OpenToolkit.Mathematics.Vector2i(resX, resY);
            nativeWindowSettings.StartVisible = true;
            nativeWindowSettings.StartFocused = true;
            _gameWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
         
            _renderer = renderer;
            _sceneManager = sceneManager;
            AddListeners();
        }

        //TODO: investigate how to rewrite into observable. RX library
        private void AddListeners()
        {
            _gameWindow.Load += () => { OnLoad(); };
            _gameWindow.Unload += () => { OnUnload(); };
            _gameWindow.RenderFrame += (FrameEventArgs args) => { OnRender(); };
            _gameWindow.UpdateFrame += (FrameEventArgs args) => { OnUpdated(); };
        }

        private void RemoveListeners()
        {
            _gameWindow.Load -= () => { OnLoad(); };
            _gameWindow.Unload -= () => { OnUnload(); };
            _gameWindow.RenderFrame -= (FrameEventArgs args) => { OnRender(); };
            _gameWindow.UpdateFrame -= (FrameEventArgs args) => { OnUpdated(); };
        }
        public void OnLoad()
        {
            _gameWindow.MakeCurrent();
            _sceneManager.OnLoad();
            _renderer.OnLoad();
            _renderer.SetViewPort(_gameWindow.ClientSize.X, _gameWindow.ClientSize.Y);
            _gameWindow.Title = _sceneManager.Scene.GetType().Name;
            LoadingComplete?.Invoke(this, null);
        }


        public void OnRender()
        {
            _sceneManager.OnRender();
            _renderer.OnRender();
            _gameWindow.SwapBuffers();
            Time += _gameWindow.RenderTime;
            _gameWindow.Title = _sceneManager.Scene.GetType().Name + " FPS: " + (1d / _gameWindow.RenderTime).ToString("0.");           
        }

        public void OnResized()
        {
        }

        public void OnUpdated()
        {
            _sceneManager.OnUpdated();
            _renderer.OnUpdated();
        }

        public void Run()
        {
            _gameWindow.Run();
        }

        public void OnUnload()
        {
            RemoveListeners();
            _gameWindow.Dispose();
        }
    }
}
