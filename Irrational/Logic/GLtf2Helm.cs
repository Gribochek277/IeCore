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
                Shader = new ShaderProg("vs.glsl", "fs.glsl", true)
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
            Position = new Vector3(0f, -2.5f, -5.0f);
            Scale = new Vector3(1f, 1f, 1f);
        }
    }
}
