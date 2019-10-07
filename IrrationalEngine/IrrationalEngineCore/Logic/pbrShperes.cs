using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Gltf2;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class PbrShperes : SceneObject, IUpdatable
    {
        public PbrShperes()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new Pbr(),
                "Resources/MetalRoughSpheres/glTF/MetalRoughSpheres.gltf",
                new Gltf2MaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(),
                "Resources/MetalRoughSpheres/glTF/MetalRoughSpheres.gltf"
                );
            BasicManipulationsComponent manipulation = new BasicManipulationsComponent(meshComponent);
            AddComponent(manipulation);
            AddComponent(material);
            AddComponent(meshComponent);           
        }
        public override void OnLoad()
        {
            base.OnLoad();
            Position = Vector3.Zero;
            Scale = new Vector3(1f, 1f, 1f);
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
