using IeCoreInterfaces.Core;
using IeCoreInterfaces.SceneObjectComponents;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjects
{
    public class SceneObject : ISceneObject
    {
        private const string ModelSceneObjectComponentName = "ModelSceneObjectComponent";
        private ILogger<SceneObject> _logger;
        private Vector3 _rotation = Vector3.Zero;
        public SceneObject(ILogger<SceneObject> logger)
        {
            _logger = logger;
        }
        public string Name { get; set; } = "New SceneObject";

        public Dictionary<string, ISceneObjectComponent> components { get; private set; } = new Dictionary<string, ISceneObjectComponent>();

        public Vector3 Scale { get; set; } = Vector3.One;
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; ApplyRotation(); } } 
        public Vector3 Position { get; set; } = Vector3.Zero;

        public void AddComponent(ISceneObjectComponent component)
        {
            components.Add(component.Name, component);
          
        }

        public void OnLoad()
        {
            foreach (var component in components)
                component.Value.OnLoad();

            _logger.LogInformation(string.Format("SceneObject {0} is loaded", this.Name));
        }

        public void OnUnload()
        {
            foreach (var component in components)
                component.Value.OnUnload();
        }

        public void RemoveComponent(string componentName)
        {
            components.Remove(componentName);
        }

        private void ApplyRotation()
        {
            if (components.TryGetValue(ModelSceneObjectComponentName, out ISceneObjectComponent sceneObjectModelComponent))
            {
                IModelComponent modelComponent = (IModelComponent)sceneObjectModelComponent;
                modelComponent.Model.Meshes.FirstOrDefault().Transform.Rotation = _rotation;
            }
        }
    }
}
