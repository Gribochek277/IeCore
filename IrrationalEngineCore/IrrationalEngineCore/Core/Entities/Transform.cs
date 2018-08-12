using OpenTK;

namespace Irrational.Core.Entities {
    public class Transform { 
        public Vector3 Position {get;set;}
        public Vector3 Rotation {get;set;}
        public Vector3 Scale {get;set;}
        public Matrix4 ModelMatrix {get;set;}
        public Matrix4 ViewProjectionMatrix {get;set;}
        public Matrix4 ModelViewProjectionMatrix {get; set;}

        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            ModelMatrix = Matrix4.Identity;
            ViewProjectionMatrix = Matrix4.Identity;
            ModelViewProjectionMatrix = Matrix4.Identity;
        }
        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public void CalculateModelMatrix () {
            ModelMatrix = Matrix4.CreateScale (Scale) * Matrix4.CreateRotationX (Rotation.X) * Matrix4.CreateRotationY (Rotation.Y) * Matrix4.CreateRotationZ (Rotation.Z) * Matrix4.CreateTranslation (Position);
        }
    }
}