using IeCoreEntities.Materials;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreOpengl.Extensions;
using IeCoreOpengl.Helpers;
using IeUtils;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Linq;
using System.Text;
using IeCoreOpengl.EngineWindow;

namespace IeCoreOpengl.Rendering
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
		private Matrix4 _projection;
		private Matrix4 _view;
		private ISceneObjectComponent _camera;

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
					 
						GL.BindVertexArray(Convert.ToUInt32(currentModelComponent.Model.VertexArrayObjectId));
						//Generate buffer on OGL side and assign id of that buffer to model.
						currentModelComponent.Model.VertexBufferObjectId = GL.GenBuffer();

						//Generate buffer on OGL side and assign id of that buffer to model.
						currentModelComponent.Model.ElementBufferId = GL.GenBuffer();

						//Get all vertices from model.
						float[] vboPositionData = currentModelComponent.GetVboPositionDataOfModel();
						uint[] indexes = currentModelComponent.GetIndexesOfModel();
	
						_logger.LogDebug("VBO Position count is: {$VboData}", vboPositionData.Length.ToString());
						_logger.LogDebug("VBO Indexes count is: {$indexesData}", indexes.Length.ToString());
						
						var sb = new StringBuilder("Vertex coordinates: ");
						sb.Append(currentModelComponent.Model.Name);
						foreach (float posData in vboPositionData)
						{
							sb.Append(' ');
							sb.Append(posData);
						}
						_logger.LogTrace(sb.ToString());
						//Load indices data to GPU.
						GL.BindBuffer(BufferTarget.ElementArrayBuffer, Convert.ToUInt32(currentModelComponent.Model.ElementBufferId));
				
						GL.BufferData(BufferTarget.ElementArrayBuffer, indexes.Length * sizeof(uint), indexes, BufferUsageHint.StaticDraw);

						if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
						{
							//Get material from scene object.
							var currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
							Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;

							GL.BindTexture(TextureTarget.Texture2D, (uint)texture.Id);


								GL.TexImage2D(TextureTarget.Texture2D,
									0,
									PixelInternalFormat.Srgb8,
									(int)texture.TextureSize.X,
									(int)texture.TextureSize.Y,
									0,
									PixelFormat.Rgba,
									PixelType.UnsignedByte,
									ref texture.Bytes[0]);

								GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


							//Bind created buffer to ArrayBuffer target.
							GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)currentModelComponent.Model.VertexBufferObjectId);

							//Load model data to GPU.
								GL.BufferData(BufferTarget.ArrayBuffer, vboPositionData.Length * sizeof(float), vboPositionData, BufferUsageHint.StaticDraw);
						
						
							GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aPosition"),
								3, VertexAttribPointerType.Float, false, 0, 0);

							//TODO: use strategy because here will be added normals and other stuff
							if (currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord") != -1)
							{
								float[] vboTextureData = currentModelComponent.GetVboTextureDataOfModel();
							
								_logger.LogDebug("VBO Texture data count is: {$textureData}", vboTextureData.Length.ToString());
								uint bufferId = currentMaterialComponent.ShaderProgram.GetBuffer("aTexCoord");
								GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
									GL.BufferData(BufferTarget.ArrayBuffer,vboTextureData.Length * sizeof(float), vboTextureData, BufferUsageHint.StaticDraw);
								
								GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord"), 
									2, VertexAttribPointerType.Float, false, 0, 0);
							}
						
							//Load animations and bones
							if (sceneObject.Components.TryGetValue(AnimationSceneObjectComponent, out _animationObjectComponent))
							{
								IAnimationComponent currentAnimationComponent = (IAnimationComponent)_animationObjectComponent;

								//Load bones ids to GPU
								if (currentMaterialComponent.ShaderProgram.GetAttributeAddress("Weights") != -1)
								{
									float[] vboBoneWeightsData = currentModelComponent.GetVboWeightsDataOfModel();
								
									_logger.LogDebug("VBO Bone wieignts count is: {$weightsData}", vboBoneWeightsData.Length.ToString());
									uint bufferId = currentMaterialComponent.ShaderProgram.GetBuffer("Weights");
									GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
										GL.BufferData(BufferTarget.ArrayBuffer, vboBoneWeightsData.Length * sizeof(float),
											vboBoneWeightsData, BufferUsageHint.DynamicDraw);
									GL.VertexAttribPointer(
										currentMaterialComponent.ShaderProgram.GetAttributeAddress("Weights"), 4,
										VertexAttribPointerType.Float, false, 0, 0);
								}

								//Load bones ids to GPU
								if (currentMaterialComponent.ShaderProgram.GetAttributeAddress("boneIds") != -1)
								{
									float[] vboBoneIdsData = currentModelComponent.GetVboBoneIdsDataOfModel().Select(x=>(float)x).ToArray();
								
									_logger.LogDebug("VBO Bone Ids count is: {$boneesData}", vboBoneIdsData.Length.ToString());
									uint bufferId = currentMaterialComponent.ShaderProgram.GetBuffer("boneIds");
									GL.BindBuffer(BufferTarget.ArrayBuffer,bufferId);
										GL.BufferData(BufferTarget.ArrayBuffer,vboBoneIdsData.Length * sizeof(float),
											vboBoneIdsData, BufferUsageHint.DynamicDraw);
									GL.VertexAttribPointer(
										currentMaterialComponent.ShaderProgram.GetAttributeAddress("boneIds"), 4,
										VertexAttribPointerType.Float, false, 0, 0);
								}
							}


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
					currentMaterialComponent.ShaderProgram.EnableVertexAttribArrays();
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

					GL.BindVertexArray((uint)currentModelComponent.Model.VertexArrayObjectId);
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, currentModelComponent.Model.ElementBufferId);


					Matrix4 modelMatrix = currentModelComponent.Model.Meshes.FirstOrDefault().Transform.ModelMatrix.ConvertToOpenTkMatrix4();
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("model"),false, ref modelMatrix);
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("projection"), false, ref _projection);
					GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("view"), false, ref _view);

					if (sceneObject.Components.TryGetValue(AnimationSceneObjectComponent, out _animationObjectComponent))
					{
						IAnimationComponent currentAnimationComponent = (IAnimationComponent)_animationObjectComponent;
						
						currentAnimationComponent.UpdateAnimation((float)OpenGlWindow.RenderFrameDeltaTime);

						Matrix4[] offsetMatrices = currentAnimationComponent.FinalBonesMatrices.Select(x => x.ConvertToOpenTkMatrix4()).ToArray();
						
						_uniformHelper.TryAddUniform(offsetMatrices, "finalBonesMatrices[0]", currentMaterialComponent.ShaderProgram);
						
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
			//throw new NotImplementedException();
		}

		public void SetViewPort(int width, int height)
		{
			_width = width;
			_height = height;
		}
	}
}
