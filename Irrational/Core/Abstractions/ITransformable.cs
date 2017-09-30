namespace Irrational.Core.Abstractions
{
    public interface ITransformable
    {
        int X { get; set; }
        int Y { get; set; }
        void OnTransform();
    }
}
