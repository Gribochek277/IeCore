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
    class Program
    {
        private static IServiceProvider _serviceProvider;
		static void Main(string[] args)
        {
            //Gltf2ModelLoader loader = new Gltf2ModelLoader();
            //loader.LoadFromFile("Resources/Gltf/Damagedhelmet/glTF/DamagedHelmet.gltf");
            RegisterServices();
            IWindow window = _serviceProvider.GetService<IWindowFactory>().Create();
            window.Run();
            DisposeServices();
        }



     private static void RegisterServices()
     {
         ServiceCollection collection = new ServiceCollection();

         collection.AddScoped<IScene, TestScene>();
         collection.AddScoped<IRenderer, OpenglRenderer>();
         collection.AddScoped<ISceneManager, SceneManager>();
         collection.AddScoped<IWindowFactory, WindowFactory>();
         collection.AddScoped<IUniformHelper, UniformHelper>();
         collection.AddScoped<IPipelineData, PipelineData>();
         
         _serviceProvider = collection.BuildServiceProvider();
     }
     private static void DisposeServices()
     {
         if(_serviceProvider == null)
         {
             return;
         }
         if (_serviceProvider is IDisposable)
         {
             ((IDisposable)_serviceProvider).Dispose();
         }
     }
	}
}
