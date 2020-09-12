using IIeCoreInterfaces.Behaviour;
using System.Collections.Generic;

namespace IeCoreInterfaces.Core
{
    /// <summary>
    /// Base interface for all objects on scene.
    /// </summary>
    public interface ISceneObject: ILoadable, IScalable, IRotatable
    {
        /// <summary>
        /// Name of scene object.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Components which are attached to this scene object.
        /// </summary>
        Dictionary<string, ISceneObjectComponent> components { get; }
        /// <summary>
        /// Adds new component to this scene object.
        /// </summary>
        /// <param name="component"></param>
        void AddComponent(ISceneObjectComponent component);
        /// <summary>
        /// Removes component from this scene object.
        /// </summary>
        /// <param name="componentname"></param>
        void RemoveComponent(string componentname);
    }
}
