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
    public class Lion : SceneObject, IUpdatable
    {
        public Lion()
        {
            MaterialSceneObjectComponent materialComponent =
                new MaterialSceneObjectComponent(
                    new Pbr(),
                    "Resources/Lion/Lion-snake.mtl",
                    new WavefrontMaterialLoader()
                    );
          

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new WavefrontModelLoader(),
                "Resources/Lion/Lion-snake.obj"
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
            Scale = new Vector3(1f, 1f, 1f) * 0.2f;
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
