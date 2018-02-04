using Irrational.Core.Entities;
using Irrational.Core.Entities.SceneObjectComponents;
using Irrational.Core.Shaders;
using Irrational.Utils;
using OpenTK;

namespace Irrational.Logic
{
    public class Knight : SceneObject
    {
        public Knight()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true),
                "Resources/knight3.mtl",
                new WavefrontMaterialLoader()
                );

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
