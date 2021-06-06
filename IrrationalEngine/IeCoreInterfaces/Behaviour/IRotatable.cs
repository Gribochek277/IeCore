using System.Numerics;

namespace IeCoreInterfaces.Behaviour
{
    /// <summary>
    /// Determines that class should support rotating behaviour.
    /// </summary>
    public interface IRotatable
    {
        /// <summary>
        /// Contains value of rotation.
        /// </summary>
        Vector3 Rotation { get; set; }
    }
}
