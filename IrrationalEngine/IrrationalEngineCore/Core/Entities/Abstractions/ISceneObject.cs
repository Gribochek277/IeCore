using System.Collections.Generic;
using IrrationalEngineCore.Core.Abstractions;

namespace IrrationalEngineCore.Core.Entities.Abstractions {
    public interface ISceneObject : ILoadable, IScalable, IRotatable, ITransformable {
        string Name { get; set; }
        Dictionary<string, ISceneObjectComponent> components { get; }
        void AddComponent (ISceneObjectComponent component);
    }
}