using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities.Abstractions;
using System.Collections.Generic;

namespace IrrationalEngineCore.Core.Renderer.Abstractions
{

    public interface IRenderer : IRenderable, IResisable, IUpdatable, ILoadable
    {
        void SetViewPort(int width, int height);
    }
}
