using OpenTK;

namespace IrrationalEngineCore.Core.Abstractions
{
    public interface ITransformable
    {
        Vector3 Position { get; set;}
        void OnTransform();
    }
}
