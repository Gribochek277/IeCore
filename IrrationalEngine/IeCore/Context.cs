using IeCoreInterfaces.Assets;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IeCore
{
    public static class Context
    {
        //Flag which signalizes that context is fully initialized and ready to use
        public static bool IsContextReady { get; set; } = false;
        public static IAssetManager Assetmanager
        {
            get {
                if (IsContextReady)
                    return IrrationalEngine.ServiceProvider.GetService<IAssetManager>();
                else
                    throw new Exception("Context not ready");
            } 
        }
    }
}
