using System.Diagnostics;
using IeCoreInterfaces;
using IeCoreInterfaces.Input;
using IeCoreInterfaces.Rendering;
using Microsoft.Extensions.Logging;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using IWindow = IeCoreInterfaces.EngineWindow.IWindow;

namespace IeCoreSilkNetOpenGl.EngineWindow;

public class SilkNetOpenGlWindow : IWindow
{
	public static GL GetWindowContext => GL.GetApi(window);

	private static Silk.NET.Windowing.IWindow window;

	private readonly ISceneManager _sceneManager;
	private readonly IRenderer _renderer;
	private readonly ILogger<SilkNetOpenGlWindow> _logger;
	private readonly IInputService? _inputService;
	private IInputContext? _inputContext;

	public event EventHandler? LoadingComplete;
	public int UpdateRate { private get; set; } = 0;
	public int FrameRate { private get; set; } = 0;

	public double RenderFrameDeltaTime { get; private set; }

	public double UpdateDeltaTime { get; private set; }

	private readonly Stopwatch _renderFrameStopwatch = new Stopwatch();

	private readonly Stopwatch _updateStopwatch = new Stopwatch();

	public SilkNetOpenGlWindow(int resX, int resY, IRenderer renderer, ISceneManager sceneManager,
		ILogger<SilkNetOpenGlWindow> logger, IInputService? inputService = null)
	{
		_renderer = renderer;
		_sceneManager = sceneManager;
		_logger = logger;
		_inputService = inputService;
		WindowOptions options = WindowOptions.Default;
		options.Size = new Vector2D<int>(resX, resY);
		options.VSync = true;
		options.ShouldSwapAutomatically = false;


		window = Window.Create(options);
		
		
		AddListeners();
	}

	private void AddListeners()
	{
		window.Load += OnLoad;
		window.Update += OnUpdated;
		window.Render += OnRender;
	}

	private void RemoveListeners()
	{
		window.Render -= OnRender;
		window.Load -= OnLoad;
		window.Update -= OnUpdated;
	}

	public void OnLoad()
	{
		window.MakeCurrent();
		_sceneManager.OnLoad();
		_renderer.SetContext(this as IWindow);
		_renderer.OnLoad();
		_renderer.SetViewPort(window.Size.X, window.Size.Y);
		window.Title = _sceneManager.Scene.GetType().Name;
		LoadingComplete?.Invoke(this, null!);

		if (_inputService != null)
		{
			_inputContext = window.CreateInput();
			foreach (IKeyboard keyboard in _inputContext.Keyboards)
				keyboard.KeyDown += OnKeyDown;
		}
	}

	public void OnUnload()
	{
		if (_inputContext != null)
		{
			foreach (IKeyboard keyboard in _inputContext.Keyboards)
				keyboard.KeyDown -= OnKeyDown;
			_inputContext.Dispose();
		}
		RemoveListeners();
		window.Dispose();
	}

	private void OnKeyDown(IKeyboard keyboard, Key key, int scanCode)
	{
		_inputService?.RaiseKeyPressed(key.ToString());
	}

	private void OnUpdated(double obj)
	{
		OnUpdated();
	}

	public void OnUpdated()
	{
		_updateStopwatch.Reset();
		_updateStopwatch.Start();

		_renderer.OnUpdated();
	}
	
	private void OnRender(double obj)
	{
		RenderFrameDeltaTime = obj;
		OnRender();
	}
	public void OnRender()
	{
		_renderer.OnRender();
		window.SwapBuffers();
		_sceneManager.OnUpdated();
		window.Title = $"{_sceneManager.Scene.GetType().Name}";
	}

	public void OnResized()
	{
		throw new NotImplementedException();
	}

	public void Run()
	{
		window.Run();
	}
}