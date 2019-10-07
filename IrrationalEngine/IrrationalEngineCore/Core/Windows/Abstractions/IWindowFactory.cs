using IrrationalEngineCore.Core.Windows.Abstractions;

namespace IrrationalEngineCore.Core.Windows.Abstractions
{
    public interface IWindowFactory
    {
        IWindow Create();
    }
}