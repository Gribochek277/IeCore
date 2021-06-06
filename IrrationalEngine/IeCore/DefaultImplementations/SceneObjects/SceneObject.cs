using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using IeCoreInterfaces;
using IeCoreInterfaces.SceneObjectComponents;

namespace IeCore.DefaultImplementations.SceneObjects
{
    public class SceneObject : ISceneObject
    {
        private const string ModelSceneObjectComponentName = "ModelSceneObjectComponent";
        private Vector3 _rotation = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

        public string Name { get; set; } = "New SceneObject";

        public Dictionary<string, ISceneObjectComponent> Components { get; private set; } = new Dictionary<string, ISceneObjectComponent>();

        public Vector3 Scale { get => _scale;
            set { _scale = value; ApplyScale(); } }
        public Vector3 Rotation { get => _rotation;
            set { _rotation = value; ApplyRotation(); } } 
        public Vector3 Position { get; set; } = Vector3.Zero;

        public void AddComponent(ISceneObjectComponent component)
        {
            if (!Components.TryAdd(component.Name, component))
            {
                throw new ArgumentException("SceneObjectComponent with same name already exists.");
            }
          
        }

        public void OnLoad()
        {
            if (Components.TryGetValue("AnimationSceneObjectComponent", out ISceneObjectComponent retrivedAnimationComponent))
            {
                if (Components.TryGetValue("ModelSceneObjectComponent", out ISceneObjectComponent modelComponent))
                {
                    IAnimationComponent animationComponent = (IAnimationComponent)retrivedAnimationComponent;
                    animationComponent.ModelComponent = (IModelComponent)modelComponent;
                }
                else
                {
                    throw new ArgumentException("IAnimationComponent requires IModelComponent");
                }
            }
             

            foreach (var component in Components)
                component.Value.OnLoad();
        }

        public void OnUnload()
        {
            foreach (var component in Components)
                component.Value.OnUnload();
        }

        public void RemoveComponent(string componentName)
        {
            Components.Remove(componentName);
        }

        private void ApplyRotation()
        {
            if (Components.TryGetValue(ModelSceneObjectComponentName, out ISceneObjectComponent sceneObjectModelComponent))
            {
                var modelComponent = (IModelComponent)sceneObjectModelComponent;
                modelComponent.Model.Meshes.FirstOrDefault().Transform.Rotation = _rotation;
            }
        }

        private void ApplyScale()
        {
            if (Components.TryGetValue(ModelSceneObjectComponentName, out ISceneObjectComponent sceneObjectModelComponent))
            {
                var modelComponent = (IModelComponent)sceneObjectModelComponent;
                modelComponent.Model.Meshes.FirstOrDefault().Transform.Scale = _scale;
            }
        }
    }
}
