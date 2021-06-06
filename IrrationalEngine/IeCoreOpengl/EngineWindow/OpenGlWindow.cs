using System;
using System.Diagnostics;
using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using Microsoft.Extensions.Logging;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace IeCoreOpengl.EngineWindow
{
    public class OpenGlWindow : IWindow
    {
        private readonly GameWindow _gameWindow; //TODO: Wrap this class with own class
        private readonly ISceneManager _sceneManager;
        private readonly IRenderer _renderer;
        private readonly ILogger<OpenGlWindow> _logger;

        public event EventHandler LoadingComplete;
        public int UpdateRate { private get; set; } = 0;
        public int FrameRate { private get; set; } = 0;

        public double RenderFrameDeltaTime { get; private set; }

        public double UpdateDeltaTime { get; private set; }

        private readonly Stopwatch _renderFrameStopwatch = new Stopwatch();

        private readonly Stopwatch _updateStopwatch = new Stopwatch();
        public OpenGlWindow(int resX, int resY, IRenderer renderer, ISceneManager sceneManager, ILogger<OpenGlWindow> logger)
        {
            var gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.IsMultiThreaded = false; //TODO: Investigate this option.
            gameWindowSettings.RenderFrequency = FrameRate;
            gameWindowSettings.UpdateFrequency = UpdateRate;
            var nativeWindowSettings = NativeWindowSettings.Default;
            nativeWindowSettings.APIVersion = new Version(3, 2);
            nativeWindowSettings.Size = new Vector2i(resX, resY);
            nativeWindowSettings.StartVisible = true;
            nativeWindowSettings.StartFocused = true;
            _gameWindow = new GameWindow(gameWindowSettings, nativeWindowSettings)
            {
                VSync = VSyncMode.Off
            };

            _logger = logger;
            _renderer = renderer;
            _sceneManager = sceneManager;

            _logger.LogDebug($"VSync : {_gameWindow.VSync}");
            _logger.LogDebug($"Window size : {_gameWindow.Size.X}x{_gameWindow.Size.Y}");
            _logger.LogDebug($"Frame rate : {FrameRate}");
            _logger.LogDebug($"Update rate : {UpdateRate}");


            AddListeners();
        }

        //TODO: investigate how to rewrite into observable. RX library
        private void AddListeners()
        {
            _gameWindow.Load += OnLoad;
            _gameWindow.Unload += OnUnload;
            _gameWindow.Resize += _ => { OnResized(); };
            _gameWindow.RenderFrame += OnRender;
            _gameWindow.UpdateFrame += OnUpdated;
        }

        private void RemoveListeners()
        {
            _gameWindow.Load -= () => { OnLoad(); };
            _gameWindow.Unload -= () => { OnUnload(); };
            _gameWindow.Resize -= _ => { OnResized(); };
            _gameWindow.RenderFrame -= args => { OnRender(args); };
            _gameWindow.UpdateFrame -= args => { OnUpdated(args); };
        }
        public void OnLoad()
        {
            _gameWindow.MakeCurrent();
            _sceneManager.OnLoad();
            _renderer.OnLoad();
            _renderer.SetViewPort(_gameWindow.ClientSize.X, _gameWindow.ClientSize.Y);
            _gameWindow.Title = _sceneManager.Scene.GetType().Name;
            LoadingComplete?.Invoke(this, null!);
        }

        private void OnRender(FrameEventArgs args)
        {
            
            _renderFrameStopwatch.Reset();
            _renderFrameStopwatch.Start();
            // args.Time;
            // _gameWindow.RenderTime;
            OnRender();
            RenderFrameDeltaTime = args.Time;

        }
        public void OnRender()
        {
            _renderer.OnRender();
            _gameWindow.SwapBuffers();
            //_logger.LogTrace($"FPS {(1d / RenderFrameDeltaTime):0.}");
            _sceneManager.OnUpdated();
            _gameWindow.Title = $"{_sceneManager.Scene.GetType().Name} FPS: {(1d / RenderFrameDeltaTime):0.}";
        }

        public void OnResized()
        {
            _logger.LogDebug($"Resized to : {_gameWindow.Size.X}x{_gameWindow.Size.Y}");
            _renderer.SetViewPort(_gameWindow.Size.X, _gameWindow.Size.Y);
            _renderer.OnResized();
        }

        private void OnUpdated(FrameEventArgs args)
        {
            UpdateDeltaTime = args.Time;
            OnUpdated();
            _logger.LogTrace($"UPS {(1d / UpdateDeltaTime):0.}");
        }

        public void OnUpdated()
        {
            _updateStopwatch.Reset();
            _updateStopwatch.Start();
          
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
