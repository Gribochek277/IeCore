using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Assimp;
using IrrationalEngineCore.Loaders.Gltf2;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class Cerberus : SceneObject, IUpdatable
    {
        public Cerberus()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new Pbr(),
               // "Resources/Gltf/sphere/sphere.gltf",
                "Resources/Gltf/cerebrus/scene.gltf",
                new Gltf2MaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(true),
            //    "Resources/Gltf/sphere/sphere.gltf"
                 "Resources/Gltf/cerebrus/scene.gltf"
                );
            BasicManipulationsComponent manipulation = new BasicManipulationsComponent(meshComponent);
            AddComponent(manipulation);
            AddComponent(material);
            AddComponent(meshComponent);           
        }
        public override void OnLoad()
        {
            base.OnLoad();
            Position = Vector3.One;
            Scale = new Vector3(1f, 1f, 1f) * 0.01f;
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
