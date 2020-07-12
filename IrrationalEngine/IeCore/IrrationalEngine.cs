using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.Window;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCore.AssetManagers;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreInterfaces.Shaders;
using IeCoreOpengl.Rendering;
using IeCoreOpengl.Shaders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IeCore
{
    public class IrrationalEngine
    {
        public static IServiceProvider ServiceProvider;
        static void Main(string[] args)
        {
            RegisterServices();
            IWindow window = ServiceProvider.GetService<IWindowFactory>().Create();
            window.Run();
            DisposeServices();
        }

        public static void RegisterServices()
        {
            ServiceCollection collection = new ServiceCollection();

            collection.AddScoped<IRenderer, OpenGLRenderer>();
            collection.AddScoped<ISceneManager, SceneManager>();
            collection.AddScoped<IWindowFactory, WindowFactory>();
            collection.AddScoped<IShaderProgram, ShaderProgram>();
            collection.AddScoped<IMaterialComponent, MaterialComponent>();
            collection.AddScoped<IAssetManager, AssetManager>();

            Context.IsContextReady = true;

            ServiceProvider = collection.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (ServiceProvider == null)
            {
                return;
            }
            if (ServiceProvider is IDisposable)
            {
                ((IDisposable)ServiceProvider).Dispose();
            }
        }
    }
}
