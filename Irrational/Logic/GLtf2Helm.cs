using Irrational.Core.Entities;
using Irrational.Core.Shaders;
using Irrational.Utils;
using OpenTK;

namespace Irrational.Logic
{
    public class GLtf2Helm : SceneObject
    {
        public GLtf2Helm()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent()
            {
                MaterialSource = "Resources/Lion/Lion-snake.mtl",
                Shader = new ShaderProg("vs_norm.glsl", "fs_PBR.glsl", true)
            };

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(),
                "Resources/Gltf/DamagedHelmet/glTF-Binary/DamagedHelmet.glb"
                );
            AddComponent(material);
            AddComponent(meshComponent);
        }
        public override void OnLoad()
        {
            base.OnLoad();
            Position = Vector3.Zero;
            Scale = new Vector3(1f, 1f, 1f);
        }
    }
}
