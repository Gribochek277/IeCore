using System.Collections.Generic;
using Irrational.Core.Abstractions;

namespace Irrational.Core.Entities.Abstractions {
    public interface IScene : ILoadable, IUpdatable, IRenderable, IResisable {
        List<ISceneObject> GetObjects { get; }
    }
}