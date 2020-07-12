using IIeCoreInterfaces.Behaviour;

namespace IeCoreInterfaces.EngineWindow
{
    /// <summary>
    /// Determines window which is opened during runtime.
    /// </summary>
    public interface IWindow : ILoadable, IUpdatable, IRenderable, IResizable
    {
        /// <summary>
        /// Run window.
        /// </summary>
        void Run();

        /// <summary>
        /// Set update rate.
        /// </summary>
        int UpdateRate { set; }

        /// <summary>
        /// Set frame rate.
        /// </summary>
        int FrameRate { set; }
    }
}
