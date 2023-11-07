using System.Numerics;
using System.Text;
using IeCoreInterfaces;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreSilkNetOpenGl.EngineWindow;
using IeCoreSilkNetOpenGl.Helpers;
using IeUtils;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;


namespace IeCoreSilkNetOpenGl.Rendering;

public class Renderer: IRenderer
{
	private static GL _gl;
	
	private static uint Vbo;
	private static uint Ebo;
	private static uint Vao;
	private static uint Shader;

	public IWindow window;
	
	private const string ModelObjectComponent = "ModelSceneObjectComponent";
	private const string MaterialObjectComponent = "MaterialSceneObjectComponent";
	private const string AnimationSceneObjectComponent = "AnimationSceneObjectComponent";
	private readonly ISceneManager _sceneManager;
	private ISceneObjectComponent _materialObjectComponent;
	private ISceneObjectComponent _modelObjectComponent;
	private ISceneObjectComponent _animationObjectComponent;
	private readonly IAssetManager _assetManager;
	private readonly ILogger<Renderer> _logger;
	
	private Matrix4x4 _projection;
	private Matrix4x4 _view;
	private ISceneObjectComponent _camera;
	
	private int _width = 600, _height = 600;


	public Renderer(ISceneManager sceneManager, IAssetManager assetManager, ILogger<Renderer> logger)
	{
		sceneManager.AssertNotNull(nameof(sceneManager));
		logger.AssertNotNull(nameof(logger));
		assetManager.AssertNotNull(nameof(assetManager));
		_sceneManager = sceneManager;
		_logger = logger;
		_assetManager = assetManager;
	}

	/*
	//Vertex shaders are run on each vertex.
	private static readonly string VertexShaderSource = @"
        #version 330 core //Using version GLSL version 3.3
        layout (location = 0) in vec4 vPos;
        
        void main()
        {
            gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
        }
        ";

	//Fragment shaders are run on each fragment/pixel of the geometry.
	private static readonly string FragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;

        void main()
        {
            FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }
        ";

	//Vertex data, uploaded to the VBO.
	private static readonly float[] Vertices =
	{
		//X    Y      Z
		0.5f,  0.5f, 0.0f,
		0.5f, -0.5f, 0.0f,
		-0.5f, -0.5f, 0.0f,
		-0.5f,  0.5f, 0.5f
	};

	//Index data, uploaded to the EBO.
	private static readonly uint[] Indices =
	{
		0, 1, 3,
		1, 2, 3
	};*/
	
	
        public unsafe void OnLoad()
        {   
	        _gl = SilkNetOpenGlWindow.GetWindowContext;
			_gl.Enable(EnableCap.DepthTest);
			_gl.ClearColor(0.6f, 0.3f, 0.3f, 1.0f);
			
			//Generate textures
			foreach (IeCoreEntities.Materials.Texture texture in _assetManager.RetrieveAll<IeCoreEntities.Materials.Texture>())
			{
				texture.Id = (int)_gl.GenTexture();
			}

			foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
			{
				//Find model component in scene object.
				if (sceneObject.Components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
				{
					//Get model from scene object.
					IModelComponent currentModelComponent = (IModelComponent) _modelObjectComponent;
					//Generate VAO on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.VertexArrayObjectId = (int) _gl.GenVertexArray();
					_gl.BindVertexArray((uint) currentModelComponent.Model.VertexArrayObjectId);
					//Generate buffer on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.VertexBufferObjectId = (int) _gl.GenBuffer();
					//Generate buffer on OGL side and assign id of that buffer to model.
					currentModelComponent.Model.ElementBufferId = (int) _gl.GenBuffer();

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
					_gl.BindBuffer(GLEnum.ElementArrayBuffer, (uint) currentModelComponent.Model.ElementBufferId);
					fixed (void* i = &indexes[0])
					{
						_gl.BufferData(GLEnum.ElementArrayBuffer, (nuint) (indexes.Length * sizeof(uint)), i,
							GLEnum.StaticDraw);
					}

				if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
				{
					//Get material from scene object.
					var currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
					IeCoreEntities.Materials.Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;
					
					_gl.ActiveTexture(TextureUnit.Texture0);
					_gl.BindTexture(TextureTarget.Texture2D, (uint)texture.Id);
					
					fixed (byte* ptr = &texture.Bytes[0])
					{
						_gl.TexImage2D(TextureTarget.Texture2D,
							0,
							InternalFormat.Srgb8,
							(uint) texture.TextureSize.X,
							(uint) texture.TextureSize.Y,
							0,
							PixelFormat.Bgra,
							PixelType.UnsignedByte,
							ptr);
					}

					_gl.GenerateMipmap(TextureTarget.Texture2D);


					//Bind created buffer to ArrayBuffer target.
					_gl.BindBuffer(GLEnum.ArrayBuffer, (uint)currentModelComponent.Model.VertexBufferObjectId);

					//Load model data to GPU.
					fixed (float* positionData = &vboPositionData[0])
					{
						_gl.BufferData(GLEnum.ArrayBuffer, (nuint) (vboPositionData.Length * sizeof(float)), positionData,
							GLEnum.StaticDraw);
					}

					_gl.VertexAttribPointer((uint)currentMaterialComponent.ShaderProgram.GetAttributeAddress("aPosition"),
						3, VertexAttribPointerType.Float, false, 0, null);
					
					if (currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord") != -1)
					{
						float[] vboTextureData = currentModelComponent.GetVboTextureDataOfModel();
						_gl.BindBuffer(GLEnum.ArrayBuffer, currentMaterialComponent.ShaderProgram.GetBuffer("aTexCoord"));
						fixed (void* textureData = &vboTextureData[0])
						{
							_gl.BufferData(GLEnum.ArrayBuffer, (uint)(vboTextureData.Length * sizeof(float)), textureData,
								GLEnum.StaticDraw);
						}

						_gl.VertexAttribPointer((uint)currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord"),
							2, VertexAttribPointerType.Float, false, 0, null);
					}
					
				}
					_projection = Matrix4x4.CreatePerspectiveFieldOfView(1.3f, _width / (float)_height, 0.1f, 120.0f);
				}
			}

/*            //Creating a vertex array.
            Vao = Gl.GenVertexArray();
            Gl.BindVertexArray(Vao);

            //Initializing a vertex buffer that holds the vertex data.
            Vbo = Gl.GenBuffer(); //Creating the buffer.
            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo); //Binding the buffer.
            fixed (void* v = &Vertices[0])
            {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (Vertices.Length * sizeof(uint)), v, BufferUsageARB.StaticDraw); //Setting buffer data.
            }

            //Initializing a element buffer that holds the index data.
            Ebo = Gl.GenBuffer(); //Creating the buffer.
            Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo); //Binding the buffer.
            fixed (void* i = &Indices[0])
            {
                Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (Indices.Length * sizeof(uint)), i, BufferUsageARB.StaticDraw); //Setting buffer data.
            }

            //Creating a vertex shader.
            uint vertexShader = Gl.CreateShader(ShaderType.VertexShader);
            Gl.ShaderSource(vertexShader, VertexShaderSource);
            Gl.CompileShader(vertexShader);

            //Checking the shader for compilation errors.
            string infoLog = Gl.GetShaderInfoLog(vertexShader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"Error compiling vertex shader {infoLog}");
            }

            //Creating a fragment shader.
            uint fragmentShader = Gl.CreateShader(ShaderType.FragmentShader);
            Gl.ShaderSource(fragmentShader, FragmentShaderSource);
            Gl.CompileShader(fragmentShader);

            //Checking the shader for compilation errors.
            infoLog = Gl.GetShaderInfoLog(fragmentShader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"Error compiling fragment shader {infoLog}");
            }

            //Combining the shaders under one shader program.
            Shader = Gl.CreateProgram();
            Gl.AttachShader(Shader, vertexShader);
            Gl.AttachShader(Shader, fragmentShader);
            Gl.LinkProgram(Shader);

            //Checking the linking for errors.
            Gl.GetProgram(Shader, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                Console.WriteLine($"Error linking shader {Gl.GetProgramInfoLog(Shader)}");
            }

            //Delete the no longer useful individual shaders;
            Gl.DetachShader(Shader, vertexShader);
            Gl.DetachShader(Shader, fragmentShader);
            Gl.DeleteShader(vertexShader);
            Gl.DeleteShader(fragmentShader);

            //Tell opengl how to give the data to the shaders.
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
            Gl.EnableVertexAttribArray(0);*/                  
        }

        public unsafe void OnRender() //Method needs to be unsafe due to draw elements.
        {
	        _gl.Clear((uint) ClearBufferMask.ColorBufferBit);
	      //  _gl.CullFace(TriangleFace.Back);
	        
	        foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects.ToList())
			{
				if (sceneObject.Components.TryGetValue("Camera", out _camera))
				{
					var camera = (ICamera)_camera;
					_view = camera.GetViewMatrix();
				}
				IMaterialComponent currentMaterialComponent = null;
				if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
				{
					//Get material from scene object.
					currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
					currentMaterialComponent.ShaderProgram.EnableVertexAttribArrays();
					IeCoreEntities.Materials.Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;

					UniformHelper.TryAddUniformTexture2D(_gl, texture.Id, "texture0", currentMaterialComponent.ShaderProgram, TextureUnit.Texture0);

					currentMaterialComponent.ShaderProgram.UseProgram();

					UniformHelper.TryAddUniform(_gl, currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseColor,
						"Color",
						currentMaterialComponent.ShaderProgram);
				}

				//Get all vertices from model.
				if (sceneObject.Components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
				{
					//Get model from scene object.
					var currentModelComponent = (IModelComponent)_modelObjectComponent;

					_gl.BindVertexArray((uint)currentModelComponent.Model.VertexArrayObjectId);
					_gl.BindBuffer(GLEnum.ElementArrayBuffer, (uint)currentModelComponent.Model.ElementBufferId);


					Matrix4x4 modelMatrix = currentModelComponent.Model.Meshes.FirstOrDefault().Transform.ModelMatrix;
					Matrix4x4 proj = _projection;
					Matrix4x4 view = _view;
					_gl.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("model"), 1, false, (float*) &modelMatrix);
					_gl.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("projection"), 1, false, (float*) &proj);
					_gl.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("view"), 1, false, (float*) &view);

					// Bind the VAO
					_gl.BindVertexArray((uint)currentModelComponent.Model.VertexArrayObjectId);

					_gl.DrawElements(PrimitiveType.Triangles, (uint)currentModelComponent.GetIndexesOfModel().Length, DrawElementsType.UnsignedInt,
						null);
				}
			}
            //Clear the color channel.
            
/*
            //Bind the geometry and shader.
            _gl.BindVertexArray(Vao);
            _gl.UseProgram(Shader);

            //Draw the geometry.
            _gl.DrawElements(PrimitiveType.Triangles, (uint) Indices.Length, DrawElementsType.UnsignedInt, null);
            */
        }



	public void OnResized()
	{
		//throw new NotImplementedException();
	}

	public void OnUpdated()
	{
		//throw new NotImplementedException();
	}

	public void OnUnload()
	{
		//throw new NotImplementedException();
	}

	public void SetContext<T>(T context)
	{
		throw new NotImplementedException();
	}

	public void SetViewPort(int width, int height)
	{
		//throw new NotImplementedException();
	}
}