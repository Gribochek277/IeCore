using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders;
using IrrationalEngineCore.Loaders.Assimp;

namespace IrrationalEngineCore.Logic
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
               new AssimpModelLoader(),
               "Resources/TestBox/Gltf/boxgltf.gltf"
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
              new AssimpModelLoader(),
              "Resources/TestBox/Gltf/boxgltf.gltf"
              );
            AddComponent(meshComponent);
            AddComponent(skyboxComponent);
        }
    }
}
