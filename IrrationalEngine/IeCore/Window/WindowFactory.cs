using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using IeCoreOpengl.EngineWindow;
using Microsoft.Extensions.Logging;

namespace IeCore.Window
{
	public class WindowFactory : IWindowFactory
	{
		private ISceneManager _sceneManager;
		private IRenderer _renderer;
		private ILogger<OpenGlWindow> _logger;

		public WindowFactory(IRenderer renderer, ISceneManager sceneManager, ILogger<OpenGlWindow> logger)
		{
			_sceneManager = sceneManager;
			_renderer = renderer;
			_logger = logger;
		}
		public IWindow Create()
		{
			return new OpenGlWindow(600, 600, _renderer, _sceneManager, _logger);
		}
	}
}
