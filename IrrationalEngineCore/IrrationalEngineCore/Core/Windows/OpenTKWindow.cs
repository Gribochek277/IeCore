using Irrational.Core.CoreManager.Abstractions;
using Irrational.Core.Windows.Abstractions;
using OpenTK;
using OpenTK.Graphics;
using System;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Core.Renderer.OpenGL;
using Irrational.Core.CoreManager;
using Irrational.Logic.Scenes;

namespace Irrational.Core.Windows
{
    public class OpenTKWindow : IWindow
    {
        private GameWindow _gameWindow;

        public ISceneManager SceneManager { get; }
        public IRenderer Renderer { get; }

        public static Rectangle Bounds;

        public bool LoadingDone { get; private set; }

        public OpenTKWindow()
        {
            _gameWindow = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Irrational");
            Renderer = new OpenglRenderer(_gameWindow);
            SceneManager = new SceneManager(Renderer, new TestScene());
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
            SceneManager.OnLoad();
            LoadingDone = true;
        }
        

        public void OnRendered()
        {
            SceneManager.OnRendered();
        }

        public void OnResized()
        {
            Bounds = _gameWindow.Bounds;
            SceneManager.OnResized();
        }

        public void OnUpdated()
        {
            SceneManager.OnUpdated();
        }
    }
}
