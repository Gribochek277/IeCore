using IeCoreInterfaces.Behaviour;

namespace IeCoreInterfaces.Rendering
{
	/// <summary>
	/// Renders all object on scene.
	/// Encapsulates all the API calls to graphic library.
	/// </summary>
	public interface IRenderer : IRenderable, IResizable, IUpdatable, ILoadable
	{
		/// <summary>
		/// Set all required context for renderer.
		/// </summary>
		/// <param name="context"></param>
		/// <typeparam name="T"></typeparam>
		void SetContext<T>(T context);
		/// <summary>
		/// Sets size of view port.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void SetViewPort(int width, int height);

		//TODO: Define more determined behaviour for this interface.
	}
}
