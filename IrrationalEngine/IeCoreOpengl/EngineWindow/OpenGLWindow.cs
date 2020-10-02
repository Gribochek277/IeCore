using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Windowing.Common;

namespace IeCoreOpengl.EngineWindow
{
    public class OpenGLWindow : IWindow
    {
        private GameWindow _gameWindow; //TODO: Wrap this class with own class
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public event EventHandler LoadingComplete;
        public int UpdateRate { private get; set; } = 0;
        public int FrameRate { private get; set; } = 0;

        public double RenderDeltaTime { get; private set; } = 0;
        public OpenGLWindow(int resX, int resY, IRenderer renderer, ISceneManager sceneManager)
        {
            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.IsMultiThreaded = false; //TODO: Investigate this option.
            gameWindowSettings.RenderFrequency = FrameRate;
            gameWindowSettings.UpdateFrequency = UpdateRate;
            NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.APIVersion = new Version(3, 2);
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(resX, resY);
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
            _gameWindow.RenderFrame += (FrameEventArgs args) => { OnRender(args); };
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
            //_gameWindow.VSync = VSyncMode.On;
            _sceneManager.OnLoad();
            _renderer.OnLoad();
            _renderer.SetViewPort(_gameWindow.ClientSize.X, _gameWindow.ClientSize.Y);
            _gameWindow.Title = _sceneManager.Scene.GetType().Name;
            LoadingComplete?.Invoke(this, null);          
        }


        public void OnRender(FrameEventArgs args)
        {
            RenderDeltaTime = args.Time;
            OnRender();
        }
        public void OnRender()
        {
            _sceneManager.OnRender();
            _renderer.OnRender();
            _gameWindow.SwapBuffers();
            _gameWindow.Title = _sceneManager.Scene.GetType().Name + " FPS: " + (1d / RenderDeltaTime).ToString("0.");
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
