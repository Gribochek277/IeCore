using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Entities.SceneObjectComponents;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Irrational.Core.Entities
{
    public class SceneObject : ISceneObject
    {
        private Vector3 _position;
        private Vector3 _scale;
        private Vector3 _rotaion;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value;
                if (_components.ContainsKey("MeshSceneObjectComponent"))
                {
                    OnTransform();
                }
            }
        }

        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value;
                if (_components.ContainsKey("MeshSceneObjectComponent"))
                    {
                    OnTransform();
                    }
                }
        }

        public Vector3 Rotation
        {
            get { return _rotaion; }
            set { _rotaion = value;
                if (_components.ContainsKey("MeshSceneObjectComponent"))
                {
                    OnTransform();
                }
            }
        }

        

        private Dictionary<string, ISceneObjectComponent> _components = new Dictionary<string, ISceneObjectComponent>();
        public Dictionary<string, ISceneObjectComponent> components { get { return _components; } }


        public void AddComponent(ISceneObjectComponent component)
        {
            _components.Add(component.GetType().Name,component);
        }

        public virtual void OnLoad()
        {
            foreach (var component in _components.Values)
                component.OnLoad();
        }
       
        public virtual void OnUnload()
        {
            throw new NotImplementedException();
        }

        public virtual void OnTransform()
        {
            MeshSceneObjectComponent meshCompnent = (MeshSceneObjectComponent)_components["MeshSceneObjectComponent"];
            meshCompnent.ModelMesh.Position = _position;
            meshCompnent.ModelMesh.Scale = _scale;
            meshCompnent.ModelMesh.Rotation = _rotaion;
        }
    }
}