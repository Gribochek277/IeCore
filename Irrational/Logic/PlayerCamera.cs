using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.Windows;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic
{
    public class PlayerCamera : SceneObject, IUpdatable
    {
        private bool isFPSmode = true;
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
            if (Mouse.GetState().IsButtonUp(MouseButton.Right)) { 
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
                    isFPSmode = false;
                    Console.WriteLine("FPS mode" + isFPSmode);
                }
                if (Keyboard.GetState().IsKeyDown(Key.E))
                {
                    isFPSmode = true;
                    Console.WriteLine("FPS mode" + isFPSmode);
                }
                if (Keyboard.GetState().IsKeyDown(Key.Space))
                {  
                    Position = Vector3.Zero;
                }                
            }
            Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            lastMousePos += delta;
            if (isFPSmode)
            { 
                Rotation = new Vector3(delta.X, delta.Y, 0);
                cam.AddRotation(delta.X, delta.Y);
            }
            
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (isFPSmode|| Mouse.GetState().IsButtonDown(MouseButton.Right))
            {                 
               Mouse.SetPosition(OpenTKWindow.Bounds.Left + OpenTKWindow.Bounds.Width / 2, OpenTKWindow.Bounds.Top + OpenTKWindow.Bounds.Height / 2);
            }
            
        }
    }
}
