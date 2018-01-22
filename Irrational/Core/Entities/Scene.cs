using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Shaders;
using OpenTK;
using System.Collections.Generic;

namespace Irrational
{
    public class Scene : IScene
    {
        private List<SceneObject> _sceneObjects = new List<SceneObject>();
        public List<SceneObject> Objects { get { return _sceneObjects; } }

        public void OnLoad()
        {
            for (int i = 0; i < 1; i++)
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

                SceneObject sceneObject = new SceneObject();
                sceneObject.AddComponent(material);
                sceneObject.AddComponent(meshComponent);
                sceneObject.OnLoad();
                //meshComponent.ModelMesh.CalculateNormals();
                sceneObject.Position += new Vector3(0 + (i * 3), 0.0f - 100, -10);
                sceneObject.Scale = new Vector3(1f, 1f, 1f) * 0.2f;
                _sceneObjects.Add(sceneObject);

            }
        }

        public void OnRendered()
        {
            throw new System.NotImplementedException();
        }

        public void OnResized()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdated()
        {
            throw new System.NotImplementedException();
        }
    }
}
