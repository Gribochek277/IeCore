using System;
using System.Collections.Generic;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Entities.Abstractions;
using OpenTK;

namespace IrrationalEngineCore.Core.Entities {
    public class SceneObject : ISceneObject {
        public string Name { get; set; }
        private Vector3 _position;
        private Vector3 _scale;
        private Vector3 _rotaion;

        public SceneObject(string name = "NewSceneObject")
        {
            Name = name;
        }

        public Vector3 Position {
            get { return _position; }
            set {
                _position = value;
                if (components.ContainsKey ("MeshSceneObjectComponent")) {
                    OnTransform ();
                }
            }
        }

        public Vector3 Scale {
            get { return _scale; }
            set {
                _scale = value;
                if (components.ContainsKey ("MeshSceneObjectComponent")) {
                    OnTransform ();
                }
            }
        }

        public Vector3 Rotation {
            get { return _rotaion; }
            set {
                _rotaion = value;
                if (components.ContainsKey ("MeshSceneObjectComponent")) {
                    OnTransform ();
                }
            }
        }
        public Dictionary<string, ISceneObjectComponent> components { get; } = new Dictionary<string, ISceneObjectComponent>();
       
        public void AddComponent (ISceneObjectComponent component) {
            components.Add (component.GetType ().Name, component);
        }

        public virtual void OnLoad () {
            foreach (var component in components.Values)
                component.OnLoad ();
        }

        public virtual void OnUnload () {
            throw new NotImplementedException ();
        }

        public virtual void OnTransform () {
            MeshSceneObjectComponent meshCompnent = (MeshSceneObjectComponent) components["MeshSceneObjectComponent"];
            meshCompnent.ModelMesh.Transform.Position = _position;
            meshCompnent.ModelMesh.Transform.Scale = _scale;
            meshCompnent.ModelMesh.Transform.Rotation = _rotaion;
        }
    }
}