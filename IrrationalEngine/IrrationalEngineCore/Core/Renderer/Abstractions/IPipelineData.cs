using System.Collections.Generic;
using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Entities.Abstractions;

namespace IrrationalEngineCore.Core.Renderer.Abstractions
{
    public interface IPipelineData: ILoadable
    {
        List<ISceneObject> Lights { get; }
        Camera Cam { get; }
        ISceneObject Skybox { get; }
        List<ISceneObject> Objects { get; }
        SkyboxSceneObjectComponent SkyboxComponent { get; }
    }
}