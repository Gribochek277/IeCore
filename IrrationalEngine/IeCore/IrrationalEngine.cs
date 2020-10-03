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
using Microsoft.Extensions.Logging;
using IeCoreOpengl.Helpers;
using IeCoreInterfaces.Core;
using IeCore.DefaultImplementations.Scene;
using IeCore.DefaultImplementations.SceneObjects;
using AutoMapper;

namespace IeCore
{
    public class IrrationalEngine
    {
        public static IServiceProvider ServiceProvider;
        static void Main(string[] args)
        {
            //TODO: consider change it to builder or smth else
            RegisterServices();
            IWindow window = ServiceProvider.GetService<IWindowFactory>().Create();
            Context.windowContext = window;
            window.Run();
            DisposeServices();
        }

        public static void RegisterServices()
        {
            ServiceCollection collection = new ServiceCollection();

            collection.AddAutoMapper(typeof(IrrationalEngine));

            collection.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
#if DEBUG
                loggerBuilder.AddConsole();
                loggerBuilder.SetMinimumLevel(LogLevel.Trace);
#endif
            });

            collection.AddScoped<ICamera, Camera>();
            collection.AddScoped<IUniformHelper, UniformHelper>();
            collection.AddScoped<IAssetManager, AssetManager>();
            collection.AddScoped<IRenderer, OpenGLRenderer>();
            collection.AddScoped<IScene, DefaultScene>();
            collection.AddScoped<ISceneManager, SceneManager>();
            collection.AddScoped<IWindowFactory, WindowFactory>();
            collection.AddScoped<IShaderProgram, ShaderProgram>();
            collection.AddScoped<IMaterialComponent, MaterialComponent>();

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
