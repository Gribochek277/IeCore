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
        private List<ISceneObject> _lights = new List<ISceneObject>();
        private Camera _cam;
        private ISceneObject _skybox = null;
        private List<ISceneObject> _objects;
        private SkyboxSceneObjectComponent _skyboxComponent;

        public PipelineData(ISceneManager sceneManager)
        {
            _objects = sceneManager.SceneObjects;
        }

        public List<ISceneObject> Lights { get => _lights; }
        public Camera Cam { get => _cam; }
        public ISceneObject Skybox { get => _skybox; }
        public List<ISceneObject> Objects { get => _objects; }
        public SkyboxSceneObjectComponent SkyboxComponent { get => _skyboxComponent; }

        public void OnLoad()
        {
             //sceneObjects which required for default render pipeline camera etc.
            List<ISceneObject> specificSceneObjects = new List<ISceneObject>();
            //gather specific objects
            foreach (ISceneObject sceneObject in _objects)
            {
                //gather all light sources
                if (sceneObject.components.ContainsKey("PointLightSceneObjectComponent"))
                {
                    Lights.Add(sceneObject);
                }
                //gather cameras
                if (sceneObject.GetType().Name == "PlayerCamera")
                {
                    _cam = (Camera)sceneObject.components["Camera"];
                    specificSceneObjects.Add(sceneObject);
                }
                //gather skybox
                if (sceneObject.GetType().Name == "Skybox")
                {
                    _skybox = sceneObject;       
                    _skyboxComponent = (SkyboxSceneObjectComponent)_skybox.components["SkyboxSceneObjectComponent"];         
                }
            }


            //remove specific objects from main collection of sceneObjects 
            foreach (SceneObject sceneObject in specificSceneObjects)
            {
                sceneObject.OnLoad();
                _objects.Remove(sceneObject);
            }

            foreach (SceneObject sceneObject in Lights)
            {
                _objects.Remove(sceneObject);
            }
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }
    }
}