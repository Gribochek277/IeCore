using Irrational.Core.Renderer.Abstractions;
using OpenTK;

namespace Irrational.Core.Windows
{
    public abstract class AbstractWindow
    {
        protected IRenderer _renderer;

        virtual protected void OnLoad()
        {
            _renderer.OnLoad();
        }

        virtual protected void OnRendered(double deltatime)
        {
            _renderer.OnRendered();
        }

        virtual protected void OnResized()
        {
            _renderer.OnResized();
        }

        virtual protected void OnUpdated(double deltatime)
        {
            _renderer.OnUpdated(deltatime);
        }

        virtual protected void OnUnload()
        {
            _renderer.OnUnload();
        }
    }
}
