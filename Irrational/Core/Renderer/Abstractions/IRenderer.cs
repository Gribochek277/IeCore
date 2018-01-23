using Irrational.Core.Abstractions;
using Irrational.Core.Entities.Abstractions;
using System.Collections.Generic;

namespace Irrational.Core.Renderer.Abstractions
{

    public interface IRenderer : IRenderable, IResisable, IUpdatable
    {
        //TODO : think how to avoid not using ILoadable interface
        void OnLoad(List<ISceneObject> sceneObjects, Camera camera);

        void OnUnload();
    }
}
