using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Loaders.Gltf2;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Loaders.Assimp;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class MiraFbx : SceneObject, IUpdatable
    {
        public MiraFbx(): base("MiraFbx")
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new Pbr(),
                "C:/Users/kpbil/source/models/mira/source/Mira2.fbx",
                new AssimpMaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new AssimpModelLoader(),
                "C:/Users/kpbil/source/models/mira/source/Mira2.fbx"
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
            Scale = new Vector3(1f, 1f, 1f) * 0.01f;
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
