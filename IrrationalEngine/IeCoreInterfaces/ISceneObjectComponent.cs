using IeCoreInterfaces.Behaviour;

namespace IeCoreInterfaces
{
    /// <summary>
    /// Base interface for all components which are attached to <see cref="ISceneObject"/>
    /// </summary>
    public interface ISceneObjectComponent: ILoadable
    {
        /// <summary>
        /// Name of scene object component
        /// </summary>
        string Name { get; }
    }
}
