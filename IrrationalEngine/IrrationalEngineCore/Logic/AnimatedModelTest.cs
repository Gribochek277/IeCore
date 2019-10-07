using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Assimp;
using IrrationalEngineCore.Loaders.Gltf2;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class AnimatedModelTest : SceneObject, IUpdatable
    {
        public AnimatedModelTest()
        {
            MaterialSceneObjectComponent materialComponent =
                new MaterialSceneObjectComponent(
                    new SimpleDiffuse(),
                    "C:/Users/kpbil/source/repos/glTF-Sample-Models/2.0/RiggedSimple/glTF/RiggedSimple.gltf",
                    new Gltf2MaterialLoader()
                    );


            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(),
                "C:/Users/kpbil/source/repos/glTF-Sample-Models/2.0/RiggedSimple/glTF/RiggedSimple.gltf"
                );
            BasicManipulationsComponent manipulation = new BasicManipulationsComponent(meshComponent);
            AddComponent(manipulation);
            AddComponent(materialComponent);
            AddComponent(meshComponent);
        }
        public override void OnLoad()
        {
            base.OnLoad();
            Position = new Vector3(0f, -2.5f, -5.0f);
            Scale = new Vector3(1f, 1f, 1f) * 0.5f;
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
