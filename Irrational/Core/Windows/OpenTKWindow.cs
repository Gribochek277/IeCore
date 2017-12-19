using Irrational.Core.Renderer;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Irrational.Core.Windows
{
    public class OpenTKWindow : AbstractWindow
    {
        private GameWindow _gameWindow;
       

        public OpenTKWindow()
        {
            
            _gameWindow = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Irrational");
            AddListeners();
            _renderer = new OpenglRenderer() { _gameWindow = _gameWindow };
            _gameWindow.Run(60, 60);
            
        }

        private void AddListeners()
        {
            _gameWindow.Load += (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload += (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame += (object o, FrameEventArgs e) => { OnRendered(e.Time); };
            _gameWindow.UpdateFrame += (object o, FrameEventArgs e) => { OnUpdated(e.Time); };
            _gameWindow.Resize += (object o, EventArgs e) => { OnResized(); };
        }

        protected override void OnLoad()
        {
            _renderer.OnLoad();
        }

        protected override void OnRendered(double deltatime)//TODO:Remove FrameEventArgs
        {
            _gameWindow.Title = "FPS: " + (1f / deltatime).ToString("0.");

            _renderer.OnRendered();
        }

        protected override void OnUpdated(double deltatime)
        {
            _renderer.OnUpdated(deltatime);
        }
      

        protected override void OnResized()
        {
            _renderer.OnResized();
        }

        protected override void OnUnload()
        {
            _gameWindow.Dispose();
        }

        private void RemoveListeners()
        {
            //TODO Check memory leak after closing window
        }
    }
}
