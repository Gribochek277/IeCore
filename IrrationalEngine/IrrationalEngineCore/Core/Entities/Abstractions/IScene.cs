using System.Collections.Generic;
using IrrationalEngineCore.Core.Abstractions;

namespace IrrationalEngineCore.Core.Entities.Abstractions {
    public interface IScene : ILoadable, IUpdatable, IRenderable, IResisable {
        List<ISceneObject> SceneObjects { get; }
    }
}