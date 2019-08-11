using IrrationalEngineCore.Core.Abstractions;

namespace IrrationalEngineCore.Core.Windows.Abstractions
{
    public interface IWindow : ILoadable, IUpdatable, IRenderable, IResisable
    {
        void Run();
    }
}
