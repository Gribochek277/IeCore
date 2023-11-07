namespace IeCoreInterfaces.EngineWindow
{
	/// <summary>
	/// Factory for window creation
	/// </summary>
	public interface IWindowFactory
	{
		/// <summary>
		/// Creates new instance of window.
		/// </summary>
		/// <returns></returns>
		IWindow CreateSilkNetWindow();
		/// <summary>
		/// Creates new instance of window.
		/// </summary>
		/// <returns></returns>
		IWindow CreateOpentkWindow();
	}
}
