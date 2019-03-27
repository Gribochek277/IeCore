using Irrational.Core.Entities;
using Irrational.Core.SceneObjectComponents;
using Irrational.Core.Shaders;
using Irrational.Loaders;

namespace Irrational.Logic
{
    public class Skybox : SceneObject
    {
        public Skybox(ShaderProg shader, string skyboxLocation = "Resources//Cubemaps"): base("CubemapSkybox")
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

        public Skybox(string skyboxLocation = "Resources//newport_loft.hdr") : base("HdriSkybox")
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
