using System.Numerics;

namespace IIeCoreInterfaces.Behaviour
{
    /// <summary>
    /// Determines that class should support transformations behaviour.
    /// </summary>
    public interface ITransformable
    {
        /// <summary>
        /// Contains position value.
        /// </summary>
        Vector3 Position { get; set;}
        /// <summary>
        /// Calls on trasfor object.
        /// </summary>
        void OnTransform();
    }
}
