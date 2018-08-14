using System.Collections.Generic;
using Irrational;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Core.SceneObjectComponents;

namespace Irrational.Core.Renderer.OpenGL
{
    public class PipelineData : IPipelineData
    {
        private List<ISceneObject> _lights = new List<ISceneObject>();
        private Camera _cam;
        private ISceneObject _skybox = null;
        private List<ISceneObject> _objects;
        private SkyboxSceneObjectComponent _skyboxComponent;

        public PipelineData(List<ISceneObject> sceneObjects)
        {
            _objects = sceneObjects;
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
                //gather cemeras
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