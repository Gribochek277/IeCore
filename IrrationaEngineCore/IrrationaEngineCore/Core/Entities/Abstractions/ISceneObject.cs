using Irrational.Core.Abstractions;
using System.Collections.Generic;

namespace Irrational.Core.Entities.Abstractions
{
    public interface ISceneObject : ILoadable, IScalable, IRotatable, ITransformable
    {
        Dictionary<string, ISceneObjectComponent> components { get; }
        void AddComponent(ISceneObjectComponent component);
    }
}
