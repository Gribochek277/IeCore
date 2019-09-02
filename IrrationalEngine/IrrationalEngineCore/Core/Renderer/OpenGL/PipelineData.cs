using System.Collections.Generic;
using IrrationalEngineCore;
using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.SceneObjectComponents;

namespace IrrationalEngineCore.Core.Renderer.OpenGL
{
    public class PipelineData : IPipelineData
    {
        public PipelineData(ISceneManager sceneManager)
        {
            Objects = sceneManager.SceneObjects;
        }

        public List<ISceneObject> Lights { get; } = new List<ISceneObject>();
        public Camera Cam { get; private set; }
        public ISceneObject Skybox { get; private set; } = null;
        public List<ISceneObject> Objects { get; }
        public SkyboxSceneObjectComponent SkyboxComponent { get; private set; }

        public void OnLoad()
        {
             //sceneObjects which required for default render pipeline camera etc.
            List<ISceneObject> specificSceneObjects = new List<ISceneObject>();
            //gather specific objects
            foreach (ISceneObject sceneObject in Objects)
            {
                //gather all light sources
                if (sceneObject.components.ContainsKey("PointLightSceneObjectComponent"))
                {
                    Lights.Add(sceneObject);
                }
                //gather cameras
                if (sceneObject.GetType().Name == "PlayerCamera")
                {
                    Cam = (Camera)sceneObject.components["Camera"];
                    specificSceneObjects.Add(sceneObject);
                }
                //gather skybox
                if (sceneObject.GetType().Name == "Skybox")
                {
                    Skybox = sceneObject;       
                    SkyboxComponent = (SkyboxSceneObjectComponent)Skybox.components["SkyboxSceneObjectComponent"];         
                }
            }


            //remove specific objects from main collection of sceneObjects 
            foreach (SceneObject sceneObject in specificSceneObjects)
            {
                sceneObject.OnLoad();
                Objects.Remove(sceneObject);
            }

            foreach (SceneObject sceneObject in Lights)
            {
                Objects.Remove(sceneObject);
            }
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }
    }
}