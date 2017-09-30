using Irrational.Core.Abstractions;

namespace Irrational.Core.Entities.Abstractions
{
    public interface ISceneObject : IRenderable, IResisable, IUpdatable, ILoadable, ITransformable
    {
        string MaterialSource { get; set; }
        string ModelSource { get; set; }
    }
}
