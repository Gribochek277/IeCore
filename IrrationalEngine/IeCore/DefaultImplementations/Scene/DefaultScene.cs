using IeCore.DefaultImplementations.SceneObjectComponents;
using IeCore.DefaultImplementations.SceneObjects;
using IeCore.DefaultImplementations.Textures;
using IeCoreEntities.Materials;
using IeCoreEntities.Model;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Primitives;
using IeCoreInterfaces.Shaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace IeCore.DefaultImplementations.Scene
{
	public class DefaultScene : IScene
	{
		private readonly ILogger<DefaultScene> _logger;
		private readonly IAssetManager _assetManager;
		private readonly IPrimitvesFactory _primitiveFactory;
		private readonly IShaderProgram _shaderProgram;
		public DefaultScene(ILogger<DefaultScene> logger, IAssetManager assetManager, IPrimitvesFactory primitiveFactory, IShaderProgram shaderProgram)
		{
			_logger = logger;
			_assetManager = assetManager;
			_primitiveFactory = primitiveFactory;
			_shaderProgram = shaderProgram;
		}

		private readonly List<ISceneObject> _sceneObjects = new List<ISceneObject>();
		//private PlayerCamera _camera;
		//protected Skybox _skybox;
		public IEnumerable<ISceneObject> SceneObjects => _sceneObjects;

		public ISceneObject MainCamera { set; private get; }

		// public SceneObject Camera { get { return _camera; } }
		public virtual void OnLoad()
		{
			//Generate and register in memory textures
			//TODO: Consider of creating stage for generating and registering all default objects.
			_assetManager.Register(DefaultTexture.CreateDefaultCheckerboard(2048, 512));
			Console.WriteLine(Environment.CurrentDirectory);
			_assetManager.RegisterFile("./Resources/FBX/3bones.fbx");

			var customSceneObject = new SceneObject { Name = "Knight" };


			var modelSceneObject = new ModelComponent(_assetManager.Retrieve<Model>("3bones.fbx"));
			var materialComponent = new MaterialComponent(_shaderProgram);
			var material = new Material("Knight", "knightFile");
			_assetManager.Register(material);

			material.DiffuseColor = new Vector4(0, 2, 0, 1);
			material.DiffuseTexture = _assetManager.Retrieve<Texture>("CheckerboardTexture_resolution_2048x2048");
			materialComponent.Materials.Add(material.Name, material);

			var animationComponent = new AnimationComponent();

			//customSceneObject.AddComponent(animationComponent);
			customSceneObject.AddComponent(modelSceneObject);
			customSceneObject.AddComponent(materialComponent);


			MainCamera = new SceneObject();
			Camera cam = new Camera();
			MainCamera.AddComponent(cam);
			_sceneObjects.Add(MainCamera);
			
			cam.Position += new Vector3(0, 0.5f, .5f);
			
			for (var i = 0; i < 1; i++)
			{
				//_sceneObjects.Add(_primitiveFactory.CreateCube());
				_sceneObjects.Add(customSceneObject);
				_sceneObjects[i].Name = "Scene object #" + i;
				_logger.LogInformation(i.ToString());
			}

			foreach (ISceneObject sceneObject in _sceneObjects)
			{
				sceneObject.OnLoad();
				sceneObject.Scale *= new Vector3(0.1f, 0.1f, 0.1f);
				
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
				/*sceneobject.Rotation +=
					new Vector3(0.0003f,
					0.0003f,
					0.0003f);*/
				
				

				//sceneobject.Scale *= new Vector3(0.3f, 0.3f, 0.3f);
			}
		}
	}
}
