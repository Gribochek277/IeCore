using System;
using OpenGL;

namespace IrrationalSpace
{
    public class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.002f;

        public Camera()
        {
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.x = (float)(Math.Sin((float)Orientation.x) * Math.Cos((float)Orientation.y));
            lookat.y = (float)Math.Sin((float)Orientation.y);
            lookat.z = (float)(Math.Cos((float)Orientation.x) * Math.Cos((float)Orientation.y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.x), 0, (float)Math.Cos((float)Orientation.x));

            Vector3 right = new Vector3(-forward.z, 0, forward.x);

            offset += x * right;
            offset += y * forward;
            offset.x += z;

            offset.Normalize();
            OpenTK.Vector3 temp = OpenTK.Vector3.Multiply(new OpenTK.Vector3((float)offset.x, (float)offset.y, (float)offset.z), MoveSpeed);
            offset = new Vector3(temp.X,temp.Y,temp.Z);

            Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.x = (Orientation.x + x) % ((float)Math.PI * 2.0f);
            Orientation.y = Math.Max(Math.Min(Orientation.y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
           
        }
    }
}

