using IeCoreEntities.Materials;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.EngineWindow;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreOpenTKOpengl.Extensions;
using IeUtils;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IeCoreOpenTKOpengl.Helpers;

namespace IeCoreOpenTKOpengl.Rendering
{
	public class OpenGlRenderer : IRenderer
	{
		private const string ModelObjectComponent = "ModelSceneObjectComponent";
		private const string MaterialObjectComponent = "MaterialSceneObjectComponent";
		private const string AnimationSceneObjectComponent = "AnimationSceneObjectComponent";
		private readonly ISceneManager _sceneManager;
		private ISceneObjectComponent _materialObjectComponent;
		private ISceneObjectComponent _modelObjectComponent;
		private ISceneObjectComponent _animationObjectComponent;
		private readonly IUniformHelper _uniformHelper;
		private readonly ILogger<OpenGlRenderer> _logger;
		private readonly IAssetManager _assetManager;
		private IWindow _window;
		private Matrix4 _projection;
		private Matrix4 _view;
		private ISceneObjectComponent _camera;
		private readonly HashSet<IAnimationComponent> _updatedAnimations = new HashSet<IAnimationComponent>();

		public OpenGlRenderer(ISceneManager sceneManager, IUniformHelper uniformHelper, IAssetManager assetManager, ILogger<OpenGlRenderer> logger)
		{
			sceneManager.AssertNotNull(nameof(sceneManager));
			uniformHelper.AssertNotNull(nameof(uniformHelper));
			logger.AssertNotNull(nameof(logger));
			assetManager.AssertNotNull(nameof(assetManager));
			_sceneManager = sceneManager;
			_uniformHelper = uniformHelper;
			_logger = logger;
			_assetManager = assetManager;
		}

		private int _width = 600, _height = 600;
		public void OnLoad()
		{
			GL.Enable(EnableCap.DepthTest);
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

			//Generate textures
			foreach (Texture texture in _assetManager.RetrieveAll<Texture>())
			{
				texture.Id = GL.GenTexture();
			}
			foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
			{
				//Find model component in scene object.
				if (sceneObject.Components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
				{
					//Get model from scene object.
					var currentModelComponent = (IModelComponent)_modelObjectComponent;
					//Generate VAO on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.VertexArrayObjectId = GL.GenVertexArray();
					GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);
					//Generate buffer on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.VertexBufferObjectId = GL.GenBuffer();
					//Generate buffer on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.ElementBufferId = GL.GenBuffer();

					//Get all vertices from model.
					float[] vboPositionData = currentModelComponent.GetVboPositionDataOfModel();
					uint[] indexes = currentModelComponent.GetIndexesOfModel();

					var sb = new StringBuilder("Vertex coordinates: ");
					sb.Append(currentModelComponent.Model.Name);
					foreach (float posData in vboPositionData)
					{
						sb.Append(' ');
						sb.Append(posData);
					}
					_logger.LogTrace(sb.ToString());
					//Load indices data to GPU.
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, currentModelComponent.Model.ElementBufferId);
					GL.BufferData(BufferTarget.ElementArrayBuffer, indexes.Length * sizeof(uint), indexes, BufferUsageHint.StaticDraw);


					if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
					{
						//Get material from scene object.
						var currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
						Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;

						int textureWidth = (int)texture.TextureSize.X;
						int textureHeight = (int)texture.TextureSize.Y;
						if (textureWidth <= 0 || textureHeight <= 0)
						{
							_logger.LogWarning("Texture {TextureName} has invalid size {Width}x{Height}; skipping upload.",
								texture.Name,
								textureWidth,
								textureHeight);
							continue;
						}

						int expectedTextureBytes = textureWidth * textureHeight * 4;
						if (texture.Bytes == null || texture.Bytes.Length < expectedTextureBytes)
						{
							_logger.LogWarning("Texture {TextureName} bytes are invalid. Expected at least {ExpectedBytes} bytes but got {ActualBytes}; skipping upload.",
								texture.Name,
								expectedTextureBytes,
								texture.Bytes?.Length ?? 0);
							continue;
						}

						GL.BindTexture(TextureTarget.Texture2D, texture.Id);
						GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
						GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

						GL.TexImage2D(TextureTarget.Texture2D,
						   0,
						   PixelInternalFormat.Srgb8Alpha8,
						   textureWidth,
						   textureHeight,
						   0,
						   PixelFormat.Rgba,
						   PixelType.UnsignedByte,
						   texture.Bytes);

						GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


						//Bind created buffer to ArrayBuffer target.
						GL.BindBuffer(BufferTarget.ArrayBuffer, currentModelComponent.Model.VertexBufferObjectId);

						//Load model data to GPU.
						GL.BufferData(BufferTarget.ArrayBuffer, vboPositionData.Length * sizeof(float), vboPositionData, BufferUsageHint.StaticDraw);

						GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aPosition"),
							3, VertexAttribPointerType.Float, false, 0, 0);

						//TODO: use strategy because here will be added normals and other stuff
						if (currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord") != -1)
						{
							float[] vboTextureData = currentModelComponent.GetVboTextureDataOfModel();
							GL.BindBuffer(BufferTarget.ArrayBuffer, currentMaterialComponent.ShaderProgram.GetBuffer("aTexCoord"));
							GL.BufferData(BufferTarget.ArrayBuffer, vboTextureData.Length * sizeof(float), vboTextureData, BufferUsageHint.StaticDraw);
							GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord"), 2, VertexAttribPointerType.Float, false, 0, 0);
						}
						if (sceneObject.Components.TryGetValue(AnimationSceneObjectComponent, out _animationObjectComponent))
						{
							// Upload per-vertex bone weights
							float[] boneWeights = currentModelComponent.GetBoneWeightsPerVertex();
							GL.BindBuffer(BufferTarget.ArrayBuffer, currentMaterialComponent.ShaderProgram.GetBuffer("Weights"));
							GL.BufferData(BufferTarget.ArrayBuffer, boneWeights.Length * sizeof(float), boneWeights, BufferUsageHint.StaticDraw);
							GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("Weights"),
								4, VertexAttribPointerType.Float, false, 0, 0);

							// Upload per-vertex bone IDs (integer attribute)
							int[] boneIds = currentModelComponent.GetBoneIdsPerVertex();
							GL.BindBuffer(BufferTarget.ArrayBuffer, currentMaterialComponent.ShaderProgram.GetBuffer("BoneIDs"));
							GL.BufferData(BufferTarget.ArrayBuffer, boneIds.Length * sizeof(int), boneIds, BufferUsageHint.StaticDraw);
							GL.VertexAttribIPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("BoneIDs"),
								4, VertexAttribIntegerType.Int, 0, IntPtr.Zero);
						}

						// Enable all vertex attrib arrays while VAO is bound so state is stored in VAO
						currentMaterialComponent.ShaderProgram.EnableVertexAttribArrays();

					}
				}

				_projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, _width / (float)_height, 0.1f, 120.0f);
			}
		}

		public void OnRender()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.CullFace(CullFaceMode.Back);


			foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects.ToList())
			{
				if (sceneObject.Components.TryGetValue("Camera", out _camera))
				{
					var camera = (ICamera)_camera;
					_view = camera.GetViewMatrix().ConvertToOpenTkMatrix4();
				}
				IMaterialComponent currentMaterialComponent = null;
				if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
				{
					//Get material from scene object.
					currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
					Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;

					_uniformHelper.TryAddUniformTexture2D(texture.Id, "texture0", currentMaterialComponent.ShaderProgram, TextureUnit.Texture0);

					currentMaterialComponent.ShaderProgram.UseProgram();

					_uniformHelper.TryAddUniform(currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseColor.ConvertToOpenTkVector(),
						"Color",
						currentMaterialComponent.ShaderProgram);
				}

				//Get all vertices from model.
				if (sceneObject.Components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
				{
					//Get model from scene object.
					var currentModelComponent = (IModelComponent)_modelObjectComponent;

					GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, currentModelComponent.Model.ElementBufferId);


					Matrix4 modelMatrix = currentModelComponent.Model.Meshes.FirstOrDefault().Transform.ModelMatrix.ConvertToOpenTkMatrix4();
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("model"), false, ref modelMatrix);
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("projection"), false, ref _projection);
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("view"), false, ref _view);

					// Update bone matrices per frame if animated
					if (sceneObject.Components.TryGetValue(AnimationSceneObjectComponent, out _animationObjectComponent))
					{
						var animComponent = (IAnimationComponent)_animationObjectComponent;

						// Only advance time once per frame even if the same object is drawn multiple times
						if (_updatedAnimations.Add(animComponent))
						{
							double deltaTime = _window?.RenderFrameDeltaTime ?? 0.016;
							animComponent.Update(deltaTime);
						}

						System.Numerics.Matrix4x4[] boneMatrices = animComponent.GetFinalBoneMatrices();
						if (boneMatrices != null && boneMatrices.Length > 0)
						{
							Matrix4[] otkBones = boneMatrices.Select(m => m.ConvertToOpenTkMatrix4()).ToArray();
							_uniformHelper.TryAddUniform(otkBones, "Bones", currentMaterialComponent.ShaderProgram);
						}
					}

					// Bind the VAO
					GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);

					GL.DrawElements(PrimitiveType.Triangles, currentModelComponent.GetIndexesOfModel().Length, DrawElementsType.UnsignedInt, 0);
				}
			}

		}

		public void OnResized()
		{
			GL.Viewport(0, 0, _width, _height);
			_projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, _width / (float)_height, 0.1f, 120.0f);
		}

		public void OnUnload()
		{

			//TODO: Clean up all buffers.
			throw new NotImplementedException();
		}

		public void OnUpdated()
		{
			_updatedAnimations.Clear();
		}

		public void SetContext<T>(T context)
		{
			if (context is IWindow window)
				_window = window;
		}

		public void SetViewPort(int width, int height)
		{
			_width = width;
			_height = height;
		}
	}
}
