using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic
{
    public class PlayerCamera : SceneObject, IUpdatable
    {
        private Vector2 lastMousePos = new Vector2();
        private Camera cam;
        public override void OnLoad()
        {
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
        public void OnUpdated()
        {
            cam = (Camera)components["Camera"];
            Position = cam.Position;
            
            if (Keyboard.GetState().IsKeyDown(Key.W))
            {
                Console.WriteLine(Position);
                cam.Move(0f, 0.1f, 0f);
            }
            if (Keyboard.GetState().IsKeyDown(Key.S))
            {
                cam.Move(0f, -0.1f, 0f);
                Console.WriteLine(Position);
            }
            if (Keyboard.GetState().IsKeyDown(Key.A))
            {
                cam.Move(-0.1f, 0f, 0f);
                Console.WriteLine(Position);
            }
            if (Keyboard.GetState().IsKeyDown(Key.D))
            {
                cam.Move(0.1f, 0f, 0f);
                Console.WriteLine(Position);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Q))
            {
                cam.Move(0f, 0f, 0.1f);
                Console.WriteLine(Position);
            }
            if (Keyboard.GetState().IsKeyDown(Key.E))
            {
                cam.Move(0f, 0f, -0.1f);
                Console.WriteLine(Position);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Space))
            {
                Console.WriteLine(Position);
                Position = Vector3.Zero;
            }

            Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            lastMousePos += delta;

            Rotation = new Vector3(delta.X, delta.Y, 0);
            cam.AddRotation(delta.X, delta.Y);
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
