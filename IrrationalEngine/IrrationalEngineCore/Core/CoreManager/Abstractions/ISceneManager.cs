using System.Collections.Generic;
using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities.Abstractions;

namespace IrrationalEngineCore.Core.CoreManager.Abstractions
{
    public interface ISceneManager : IRenderable, IResisable, IUpdatable, ILoadable
    {
        IScene Scene {get;}
        List<ISceneObject> SceneObjects { get;}
    }
}
