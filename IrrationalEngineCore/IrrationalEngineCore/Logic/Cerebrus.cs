using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.SceneObjectComponents;
using Irrational.Core.Shaders;
using Irrational.Utils;
using IrrationalEngineCore.Core.Shaders;
using OpenTK;
using static IrrationalEngineCore.Core.Shaders.Pbr;

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
                new Gltf2MaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(),
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
