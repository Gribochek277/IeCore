using IeCoreInterfaces;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using IeCoreOpenTKOpengl.EngineWindow;
using IeCoreSilkNetOpenGl.EngineWindow;
using Microsoft.Extensions.Logging;

namespace IeCore.Window
{
	public class WindowFactory : IWindowFactory
	{
		private ISceneManager _sceneManager;
		private IRenderer _renderer;
		private ILogger<SilkNetOpenGlWindow> _logger;
		private ILogger<OpenGlWindow> _logger2;

		public WindowFactory(IRenderer renderer, ISceneManager sceneManager, ILogger<SilkNetOpenGlWindow> logger,
			ILogger<OpenGlWindow> logger2)
		{
			_sceneManager = sceneManager;
			_renderer = renderer;
			_logger = logger;
			_logger2 = logger2;
		}
		public IWindow CreateSilkNetWindow()
		{
			return new SilkNetOpenGlWindow(600, 600, _renderer, _sceneManager, _logger);
		}

		public IWindow CreateOpentkWindow()
		{
			return new OpenGlWindow(800, 600, _renderer, _sceneManager, _logger2);
		}
	}
}
