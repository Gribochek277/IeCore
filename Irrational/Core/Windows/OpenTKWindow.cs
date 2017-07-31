using OpenTK;
using System;

namespace Irrational.Core.Windows
{
    public class OpenTKWindow : AbstractWindow
    {
        private GameWindow _gameWindow;

        public OpenTKWindow()
        {
            _gameWindow = new GameWindow();
            AddListeners();
        }

        private void AddListeners()
        {
            _gameWindow.Load += (object o, EventArgs e) => { OnLoad(); };
            _gameWindow.Unload += (object o, EventArgs e) => { OnUnload(); };
            _gameWindow.RenderFrame += (object o, FrameEventArgs e) => { OnRendered(); };
            _gameWindow.Resize += (object o, EventArgs e) => { OnResized(); };
        }

        private void RemoveListeners()
        {
            //TODO Check memory lick after closing window
        }
    }
}
