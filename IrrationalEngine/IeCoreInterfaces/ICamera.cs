using System.Numerics;
using IeCoreInterfaces.Behaviour;

namespace IeCoreInterfaces
{
    /// <summary>
    /// Defines camera scene object.
    /// </summary>
    public interface ICamera : ISceneObjectComponent, IUpdatable
    {
        /// <summary>
        /// Move camera according to coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void Move(float x, float y, float z);
        /// <summary>
        /// Rotate camera according to coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void AddRotation(float x, float y);
        /// <summary>
        /// Returns view matrix calculated for camera.
        /// </summary>
        /// <returns></returns>
        Matrix4x4 GetViewMatrix();
    }
}
