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
            SkyboxSceneObjectComponent skyboxComponent =
                new SkyboxSceneObjectComponent(skyboxLocation, 
                                               shader,
                                               SkyboxSceneObjectComponent.SkyboxType.cubemap);
            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
               new WavefrontModelLoader(),
               "Resources/TestBox/ObjFormat/box.obj"
               );
            AddComponent(meshComponent);
            AddComponent(skyboxComponent);
        }

        public Skybox(string skyboxLocation = "Resources//HDR//MonValley_DirtRoad//MonValley_G_DirtRoad_3k.hdr")
        {
            SkyboxSceneObjectComponent skyboxComponent = 
                new SkyboxSceneObjectComponent(skyboxLocation, 
                                               SkyboxSceneObjectComponent.SkyboxType.hdr);
            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
              new WavefrontModelLoader(),
              "Resources/TestBox/ObjFormat/box.obj"
              );
            AddComponent(meshComponent);
            AddComponent(skyboxComponent);
        }
    }
}
