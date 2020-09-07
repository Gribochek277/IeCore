using IIeCoreInterfaces.Behaviour;
using System.Collections.Generic;

namespace IeCoreInterfaces.Core
{
    /// <summary>
    /// Scene contains all scene objects.
    /// </summary>
    public interface IScene : ILoadable, IUpdatable, IRenderable, IResizable
    {
        /// <summary>
        /// Contains all the scene objects which are related to this scene.
        /// </summary>
        IEnumerable<ISceneObject> SceneObjects { get; }
        /// <summary>
        /// Set main camera for scene.
        /// </summary>
        ISceneObject MainCamera { set; }
}
}
