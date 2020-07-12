using System.Numerics;

namespace IIeCoreInterfaces.Behaviour
{
    /// <summary>
    /// Determines that class should support scale behaviour.
    /// </summary>
    public interface IScalable
    {
        /// <summary>
        /// Conatins scale value.
        /// </summary>
        Vector3 Scale { get; set; }
    }
}
