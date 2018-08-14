using System.Collections.Generic;
using Irrational.Core.Abstractions;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.SceneObjectComponents;

namespace Irrational.Core.Renderer.Abstractions
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