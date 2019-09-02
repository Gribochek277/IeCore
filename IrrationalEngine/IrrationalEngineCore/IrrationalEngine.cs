using System;
using IrrationalEngineCore.Core.CoreManager;
using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.Renderer.OpenGL;
using IrrationalEngineCore.Core.Renderer.OpenGL.Helpers;
using IrrationalEngineCore.Core.Windows.Abstractions;
using IrrationalEngineCore.Logic.Scenes;
using IrrationalEngineCore.Core.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace IrrationalEngineCore
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

        public static void RunProgram()
        {
            RegisterServices();
        }

     private static void RegisterServices()
     {
         ServiceCollection collection = new ServiceCollection();

         collection.AddScoped<IScene, TestLinuxScene>();
         collection.AddScoped<IRenderer, OpenglRenderer>();
         collection.AddScoped<ISceneManager, SceneManager>();
         collection.AddScoped<IWindowFactory, WindowFactory>();
         collection.AddScoped<IUniformHelper, UniformHelper>();
         collection.AddScoped<IPipelineData, PipelineData>();
         
         ServiceProvider = collection.BuildServiceProvider();
     }
     private static void DisposeServices()
     {
         if(ServiceProvider == null)
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
