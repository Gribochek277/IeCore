using Irrational.Core.Entities;
using Irrational.Core.Entities.SceneObjectComponents;
using Irrational.Core.Shaders;
using Irrational.Utils;

namespace Irrational.Logic
{
    public class Skybox : SceneObject
    {
        public Skybox(ShaderProg shader, string skyboxLocation = "Resources\\Cubemaps")
        {
            SkyboxSceneObjectComponent skyboxComponent = new SkyboxSceneObjectComponent(skyboxLocation, shader, SkyboxSceneObjectComponent.SkyboxType.cubemap);
            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
               new WavefrontModelLoader(),
               "Resources/testBox/ObjFormat/box.obj"
               );
            AddComponent(meshComponent);
            AddComponent(skyboxComponent);
        }

        public Skybox(string skyboxLocation = "Resources\\Cubemaps")
        {
            SkyboxSceneObjectComponent skyboxComponent = new SkyboxSceneObjectComponent(skyboxLocation, SkyboxSceneObjectComponent.SkyboxType.cubemap);
            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
              new WavefrontModelLoader(),
              "Resources/testBox/ObjFormat/box.obj"
              );
            AddComponent(meshComponent);
            AddComponent(skyboxComponent);
        }
    }
}
