using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Assimp;
using OpenTK;

namespace Irrational.Logic
{
    public class Cerebrus : SceneObject, IUpdatable
    {
        public Cerebrus()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new Pbr(),
               // "Resources/Gltf/sphere/sphere.gltf",
                "Resources/Gltf/cerebrus/scene.gltf",
                new AssimpMaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new AssimpModelLoader(),
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
