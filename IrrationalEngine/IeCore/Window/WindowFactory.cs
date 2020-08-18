using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using IeCoreOpengl.EngineWindow;

namespace IeCore.Window
{
    public class WindowFactory : IWindowFactory
    {
        private ISceneManager _sceneManager;
        private IRenderer _renderer;

        public WindowFactory(IRenderer renderer, ISceneManager sceneManager)
        {
            _sceneManager = sceneManager;
            _renderer = renderer;
        }
        public IWindow Create()
        {
            return new OpenGLWindow(800, 600,  _renderer, _sceneManager);
        }
    }
}
