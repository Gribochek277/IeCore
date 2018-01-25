using Irrational.Core.Entities;
using Irrational.Shaders;
using OpenTK;

namespace Irrational.Logic
{
    public class Knight : SceneObject
    {
        public Knight()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent()
            {
                MaterialSource = "Resources/knight3.mtl",
                Shader = new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true)
            };

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new WavefrontModelLoader(),
                "Resources/knight3.obj1"
                );
            AddComponent(material);
            AddComponent(meshComponent);
        }

        public override void OnLoad()
        {
            base.OnLoad();
            Position = new Vector3(0f, 0f, 0f);
            Scale = new Vector3(1f, 1f, 1f);
        }
    }
}
