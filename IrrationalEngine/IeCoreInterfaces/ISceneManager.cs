using IeCoreInterfaces.Core;
using IIeCoreInterfaces.Behaviour;

namespace IeCoreInterfaces
{
    /// <summary>
    /// Manages scenes during runtime
    /// </summary>
    public interface ISceneManager : IRenderable, IResizable, IUpdatable, ILoadable
    {
        /// <summary>
        /// Get current scene.
        /// </summary>
        IScene Scene { get; }
    }
}
