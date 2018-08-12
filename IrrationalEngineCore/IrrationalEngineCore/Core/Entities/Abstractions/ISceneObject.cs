using System.Collections.Generic;
using Irrational.Core.Abstractions;

namespace Irrational.Core.Entities.Abstractions {
    public interface ISceneObject : ILoadable, IScalable, IRotatable, ITransformable {
        Dictionary<string, ISceneObjectComponent> components { get; }
        void AddComponent (ISceneObjectComponent component);
    }
}