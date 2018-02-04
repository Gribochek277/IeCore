using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.Entities.SceneObjectComponents;
using Irrational.Core.Shaders;
using Irrational.Utils;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic
{
    public class GLtf2Helm : SceneObject, IUpdatable
    {
        public GLtf2Helm()
        {
            MaterialSceneObjectComponent material = new MaterialSceneObjectComponent(
                new ShaderProg("vs_norm.glsl", "fs_normal.glsl", true),
                "Resources/Gltf/DamagedHelmet/glTF/DamagedHelmet.gltf",
                new Gltf2MaterialLoader());

            MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                new Gltf2ModelLoader(),
                "Resources/Gltf/DamagedHelmet/glTF/DamagedHelmet.gltf"
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
            Scale = new Vector3(1f, 1f, 1f);
        }

        public void OnUpdated()
        {
            var manipulate = (BasicManipulationsComponent)components["BasicManipulationsComponent"];
            manipulate.OnUpdated();
        }
    }
}
