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
        /// Sets size of view port.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetViewPort(int width, int height);

        //TODO: Define more determined behaviour for this interface.
    }
}
