using Irrational.Core.Abstractions;

namespace Irrational.Core.Entities.Abstractions
{
    public interface ISceneObject : IRenderable, IScalable, IUpdatable, ILoadable, ITransformable, IRotatable
    {
        string MaterialSource { get; set; }
        string ModelSource { get; set; }
        Volume ModelMesh { get; set; }
    }
}
