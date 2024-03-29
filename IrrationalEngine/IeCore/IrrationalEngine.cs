﻿using AutoMapper;
using IeCore.AssetImporters;
using IeCore.AssetManagers;
using IeCore.DefaultImplementations.Primitives.Factory;
using IeCore.DefaultImplementations.Scene;
using IeCore.DefaultImplementations.SceneObjects;
using IeCore.Window;
using IeCoreInterfaces;
using IeCoreInterfaces.AssetImporters;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Primitives;
using IeCoreInterfaces.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreInterfaces.Shaders;
using IeCoreOpenTKOpengl.Helpers;
using IeCoreOpenTKOpengl.Rendering;
using IeCoreOpenTKOpengl.Shaders;
using IeCoreSilkNetOpenGl.Rendering;
using IeCoreSilkNetOpenGl.Shaders;
using UniformHelper = IeCoreSilkNetOpenGl.Helpers.UniformHelper;

namespace IeCore
{
	public class IrrationalEngine
	{
		private static IServiceProvider _serviceProvider;

		private static void Main()
		{
			//TODO: consider change it to builder or smth else
			RegisterServices();
			IWindow window = _serviceProvider.GetService<IWindowFactory>().CreateOpentkWindow();
			window.Run();
			DisposeServices();
		}

		private static void RegisterServices()
		{
			var collection = new ServiceCollection();

			collection.AddAutoMapper(typeof(IrrationalEngine));

			collection.AddLogging(loggerBuilder =>
			{
				loggerBuilder.ClearProviders();
#if DEBUG
				loggerBuilder.AddConsole();
				loggerBuilder.SetMinimumLevel(LogLevel.Debug);
#endif
			});

			collection.AddScoped<ICamera, Camera>();
			collection.AddScoped<IUniformHelper, IeCoreOpenTKOpengl.Helpers.UniformHelper>();
			collection.AddScoped<IModelImporter, ModelImporter>();
			collection.AddScoped<ITextureImporter, TextureImporter>();
			collection.AddScoped<IAssetManager, AssetManager>();
			collection.AddScoped<IRenderer, OpenGlRenderer>();
			collection.AddScoped<IPrimitvesFactory, PrimitvesFactory>();
			collection.AddScoped<IScene, DefaultScene>();
			collection.AddScoped<ISceneManager, SceneManager>();
			collection.AddScoped<IWindowFactory, WindowFactory>();
			collection.AddScoped<IShaderProgram, OpenTkShaderProgram>();
			collection.AddScoped<IMaterialComponent, MaterialComponent>();

			_serviceProvider = collection.BuildServiceProvider();
		}
		private static void DisposeServices()
		{
			switch (_serviceProvider)
			{
				case null:
					return;
				case IDisposable disposable:
					disposable.Dispose();
					break;
			}
		}
	}
}
