using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCore.DefaultImplementations.Textures;
using IeCoreEntities.Materials;
using IeCoreEntities.Model;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Input;
using IeCoreInterfaces.Primitives;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreInterfaces.Shaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace IeCore.DefaultImplementations.Scene
{
	public class DefaultScene : IScene
	{
		private readonly ILogger<DefaultScene> _logger;
		private readonly IAssetManager _assetManager;
		private readonly IPrimitvesFactory _primitiveFactory;
		private readonly IShaderProgram _shaderProgram;
		private readonly IInputService _inputService;
		public DefaultScene(ILogger<DefaultScene> logger, IAssetManager assetManager, IPrimitvesFactory primitiveFactory, IShaderProgram shaderProgram, IInputService inputService)
		{
			_logger = logger;
			_assetManager = assetManager;
			_primitiveFactory = primitiveFactory;
			_shaderProgram = shaderProgram;
			_inputService = inputService;
			_inputService.KeyPressed += OnKeyPressed;
		}

		private readonly List<ISceneObject> _sceneObjects = new List<ISceneObject>();
		//private PlayerCamera _camera;
		//protected Skybox _skybox;
		public IEnumerable<ISceneObject> SceneObjects => _sceneObjects;

		private IAnimationComponent _animationComponent;

		public ISceneObject MainCamera { set; private get; }

		// public SceneObject Camera { get { return _camera; } }
		public virtual void OnLoad()
		{
			//Generate and register in memory textures
			//TODO: Consider of creating stage for generating and registering all default objects.
			_assetManager.Register(DefaultTexture.CreateDefaultCheckerboard(2048, 512));
			Console.WriteLine(Environment.CurrentDirectory);
			_assetManager.RegisterFile($"Resources{Path.DirectorySeparatorChar}Json{Path.DirectorySeparatorChar}knight.json");

			var customSceneObject = new SceneObject { Name = "Knight" };


			var modelSceneObject = new ModelComponent(_assetManager.Retrieve<Model>("knight.fbx"));
			var materialComponent = new MaterialComponent(_shaderProgram, useAnimatedShader: true);
			var material = new Material("Knight", "knightFile");
			_assetManager.Register(material);

			material.DiffuseColor = new Vector4(0, 2, 0, 1);
			material.DiffuseTexture = _assetManager.Retrieve<Texture>("CheckerboardTexture_resolution_2048x2048");
			materialComponent.Materials.Add(material.Name, material);

			var animationComponent = new AnimationComponent();
			animationComponent.ModelComponent = modelSceneObject;
			_animationComponent = animationComponent;

			customSceneObject.AddComponent(animationComponent);
			customSceneObject.AddComponent(modelSceneObject);
			customSceneObject.AddComponent(materialComponent);


			MainCamera = new SceneObject();
			MainCamera.AddComponent(new Camera());
			_sceneObjects.Add(MainCamera);
			for (var i = 0; i < 3; i++)
			{
				//_sceneObjects.Add(_primitiveFactory.CreateCube());
				_sceneObjects.Add(customSceneObject);
				_sceneObjects[i].Name = "Scene object #" + i;
				_logger.LogInformation(i.ToString());
			}
			_sceneObjects.Add(customSceneObject);

			foreach (ISceneObject sceneObject in _sceneObjects)
			{
				sceneObject.OnLoad();
				sceneObject.Scale *= new Vector3(0.5f, 0.5f, 0.5f);
			}
		}

		public void OnRender()
		{
			throw new NotImplementedException();
		}

		public void OnResized()
		{
			throw new NotImplementedException();
		}

		public void OnUnload()
		{
			throw new NotImplementedException();
		}

		public void OnUpdated()
		{
			
			foreach (ISceneObject sceneobject in SceneObjects)
			{
				sceneobject.Rotation +=
					new Vector3(0.0003f,
					0.00003f,
					0.000003f);
			}
		}

		private void OnKeyPressed(string key)
		{
			if (_animationComponent == null) return;
			int count = _animationComponent.AnimationCount;
			if (count == 0) return;
			if (key == "Right" || key == "Up")
				_animationComponent.AnimationIndex = (_animationComponent.AnimationIndex + 1) % count;
			else if (key == "Left" || key == "Down")
				_animationComponent.AnimationIndex = (_animationComponent.AnimationIndex - 1 + count) % count;
			_logger.LogInformation("[Animation] {Index}/{Count}", _animationComponent.AnimationIndex + 1, count);
		}
	}
}
