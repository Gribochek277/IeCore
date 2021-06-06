using System.Numerics;

namespace IeCoreEntities.Model
{
    /// <summary>
    /// Stores all transformation data for models.
    /// </summary>
    public class Transform
    {
        /// <summary>
        /// Stores position.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Stores rotation.
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// Stores scale.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Stores model matrix.
        /// </summary>
        public Matrix4x4 ModelMatrix
        {
            get => CalculateModelMatrix();
            set {
            }
        }

        /// <summary>
        /// Stores viewProjection matrix.
        /// </summary>
        public Matrix4x4 ViewProjectionMatrix { get; set; }
        /// <summary>
        /// Stores MVP matrix.
        /// </summary>
        public Matrix4x4 ModelViewProjectionMatrix { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            ModelMatrix = Matrix4x4.Identity;
            ViewProjectionMatrix = Matrix4x4.Identity;
            ModelViewProjectionMatrix = Matrix4x4.Identity;
        }
        /// <summary>
        /// Calculates the model matrix from transforms
        /// </summary>
        public Matrix4x4 CalculateModelMatrix()
        {
            return ModelMatrix =
            Matrix4x4.CreateScale(Scale)
            * Matrix4x4.CreateRotationX(Rotation.X) 
            * Matrix4x4.CreateRotationY(Rotation.Y) 
            * Matrix4x4.CreateRotationZ(Rotation.Z)
            * Matrix4x4.CreateTranslation(Position);
        }
    }
}
