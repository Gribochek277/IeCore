using IeCoreInterfaces.Assets;
using IeCoreInterfaces.EngineWindow;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IeCore
{
    public static class Context
    {
        private const string ContextNotReadyError = "Context not ready";
        public static IWindow windowContext { set; private get; }

        //Flag which signalizes that context is fully initialized and ready to use
        public static bool IsContextReady { get; set; } = false;
        public static IAssetManager Assetmanager
        {
            get {
                if (IsContextReady)
                    return IrrationalEngine.ServiceProvider.GetService<IAssetManager>();
                else
                    throw new Exception(ContextNotReadyError);
            } 
        }

        public static double RendrerDeltaTime {
            get
            {
                if (IsContextReady)
                    return windowContext.RenderDeltaTime;
                else
                    throw new Exception(ContextNotReadyError);
            }
        }
    }
}
