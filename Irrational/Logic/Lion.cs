using Irrational.Core.Entities;
using Irrational.Shaders;
using OpenTK;

namespace Irrational.Logic
{
    public class Lion : SceneObject
    {
        public Lion()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent()
            {
                MaterialSource = "Resources/Lion/Lion-snake.mtl",
                Shader = new ShaderProg("vs_norm.glsl", "fs_PBR.glsl", true)
            };

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new WavefrontModelLoader(),
                "Resources/Lion/Lion-snake.obj"
                );
            AddComponent(material);
            AddComponent(meshComponent);
        }
        public override void OnLoad()
        {            
            base.OnLoad();
            Position = new Vector3(0f, -2.5f, -5.0f);
            Scale = new Vector3(1f, 1f, 1f) * 0.2f;
        }
    }
}
