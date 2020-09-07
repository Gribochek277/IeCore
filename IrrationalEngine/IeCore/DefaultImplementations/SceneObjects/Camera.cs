using IeCoreInterfaces;
using System;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjects
{
    public class Camera : ICamera
    {
        public Vector3 Position = Vector3.Zero + new Vector3(0,0,1);
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.002f;

        private Vector3 _previousPosition = Vector3.One;
        private Vector3 _previousOrientation = new Vector3((float)Math.PI, 1f, 1f);
        private Matrix4x4 _cachedViewMatrix;

        public string Name => "Camera";

        public Camera()
        {
        }

        public Matrix4x4 GetViewMatrix()
        {
            //Small optimization.
            if (Position == _previousPosition && Orientation == _previousOrientation)
                return _cachedViewMatrix;

            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y));
            lookat.Y = (float)Math.Sin(Orientation.Y);
            lookat.Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y));

            
            _cachedViewMatrix = Matrix4x4.CreateLookAt(Position, Position + lookat, Vector3.UnitY);
            _previousPosition = Position;
            _previousOrientation = Orientation;
            return _cachedViewMatrix;
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(Orientation.X), 0, (float)Math.Cos(Orientation.X));

            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.X += z;

            offset = Vector3.Normalize(offset);
            Vector3 temp = Vector3.Multiply(new Vector3(offset.X, offset.Y, offset.Z), MoveSpeed);
            offset = new Vector3(temp.X, temp.Y, temp.Z);

            Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);

        }

        public void OnLoad()
        {
            
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        public void OnUpdated()
        {
            throw new NotImplementedException();
        }
    }
}
