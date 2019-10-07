using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Loaders.Gltf2;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Assimp;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class GLtf2Helm : SceneObject, IUpdatable
    {
        public GLtf2Helm(): base("Gltf2Helm")
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new Pbr(),
                "Resources/Gltf/DamagedHelmet/glTF/DamagedHelmet.gltf",
                new Gltf2MaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new AssimpModelLoader(),
                "Resources/Gltf/DamagedHelmet/glTF/DamagedHelmet.gltf"
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
