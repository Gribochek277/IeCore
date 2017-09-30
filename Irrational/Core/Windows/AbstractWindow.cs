using Irrational.Core.Renderer.Abstractions;

namespace Irrational.Core.Windows
{
    public abstract class AbstractWindow
    {
        protected IRenderer _renderer;

        virtual protected void OnLoad()
        {
            _renderer.OnLoad();
        }

        virtual protected void OnRendered()
        {
            _renderer.OnRendered();
        }

        virtual protected void OnResized()
        {
            _renderer.OnResized();
        }

        virtual protected void OnUpdated()
        {
            _renderer.OnUpdated();
        }

        virtual protected void OnUnload()
        {
            _renderer.OnUnload();
        }
    }
}
