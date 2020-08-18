using IeCoreInterfaces.Core;
using IIeCoreInterfaces.Behaviour;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjects
{
    public class SceneObject : ISceneObject
    {
        private ILogger<SceneObject> _logger;
        public SceneObject(ILogger<SceneObject> logger)
        {
            _logger = logger;
        }
        public string Name { get; set; } = "New SceneObject";

        public Dictionary<string, ISceneObjectComponent> components { get; private set; } = new Dictionary<string, ISceneObjectComponent>();
        private List<string> _transformableComponents = new List<string>();

        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Position { get; set; }

        public void AddComponent(ISceneObjectComponent component)
        {
            components.Add(component.Name, component);
            if (typeof(ITransformable).IsAssignableFrom(component.GetType()))
            {
                _transformableComponents.Add(component.Name);
            }
        }

        public void OnLoad()
        {
            foreach (var component in components)
                component.Value.OnLoad();

            _logger.LogInformation(string.Format("SceneObject {0} is loaded", this.Name));
        }

        public void OnTransform()
        {
            foreach (string transformableComponentName in _transformableComponents)
                throw new NotImplementedException();   
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
    }
}
