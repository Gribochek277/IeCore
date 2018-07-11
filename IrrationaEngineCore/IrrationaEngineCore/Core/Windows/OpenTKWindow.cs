using Irrational.Core.CoreManager.Abstractions;
using Irrational.Core.Windows.Abstractions;
using OpenTK;
using OpenTK.Graphics;
using System;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Core.Renderer.OpenGL;
using Irrational.Core.CoreManager;
using System.Drawing;

namespace Irrational.Core.Windows
{
    public class OpenTKWindow : IWindow
    {
        private GameWindow _gameWindow;
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public static OpenTK.Rectangle Bounds;

        public OpenTKWindow()
        {
            _gameWindow = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Irrational");
            _renderer = new OpenglRenderer(_gameWindow);
            _sceneManager = new SceneManager(_renderer);
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
            _gameWindow.Dispose();
        }

        public void RemoveListeners()
        {
            //TODO Check memory leak after closing window
        }

        public void OnLoad()
        {
            Bounds = _gameWindow.Bounds;
            _sceneManager.OnLoad();
        }

        public void OnRendered()
        {
            _sceneManager.OnRendered();
        }

        public void OnResized()
        {
            Bounds = _gameWindow.Bounds;
            _sceneManager.OnResized();
        }

        public void OnUpdated()
        {
            _sceneManager.OnUpdated();
        }
    }
}
