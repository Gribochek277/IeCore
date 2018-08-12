using Irrational.Core.Abstractions;
using Irrational.Core.Entities.Abstractions;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Core.SceneObjectComponents
{
    class BasicManipulationsComponent : ISceneObjectComponent, IUpdatable
    {
        private Vector2 lastMousePos = new Vector2();
        MeshSceneObjectComponent _mesh;
        public BasicManipulationsComponent(MeshSceneObjectComponent Mesh)
        {
            _mesh = Mesh;
        }
        public void OnLoad()
        {
            
        }

        public void OnUnload()
        {
            
        }

        public void OnUpdated()
        {
            Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            lastMousePos += delta;
           
            if (Mouse.GetState().IsButtonDown(MouseButton.Right)&& Mouse.GetState().IsButtonUp(MouseButton.Left))
            {
                delta = delta * 0.001f;
                _mesh.ModelMesh.Rotation += new Vector3(delta.Y, delta.X, 0);
                Console.WriteLine(_mesh.ModelMesh.Rotation);
            }
            if (Mouse.GetState().IsButtonDown(MouseButton.Right) && Mouse.GetState().IsButtonDown(MouseButton.Left))
            {
                delta = delta * 0.001f;
                _mesh.ModelMesh.Position += new Vector3(delta.X, delta.Y, 0);
                if(Keyboard.GetState().IsKeyDown(Key.W))
                {
                    _mesh.ModelMesh.Position += new Vector3(0, 0, 0.01f);
                }
                if (Keyboard.GetState().IsKeyDown(Key.S))
                {
                    _mesh.ModelMesh.Position -= new Vector3(0, 0, 0.01f);
                }
                Console.WriteLine(_mesh.ModelMesh.Rotation);
            }
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
