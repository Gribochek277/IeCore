using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.Windows;
using IrrationalEngineCore.Core.Windows.Abstractions;
using OpenTK;
using OpenTK.Graphics;

namespace IrrationalEngineCore.Core.Windows
{
    public class WindowFactory : IWindowFactory
    {
        private GameWindow _gameWindow;
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public WindowFactory(IRenderer renderer, ISceneManager sceneManager)
        {
            _sceneManager = sceneManager;
            _renderer = renderer;
        }
        public IWindow Create()
        {
             _gameWindow = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Irrational engine");
            return new OpenGLWindow(_gameWindow,_renderer,_sceneManager);
        }
    }
}