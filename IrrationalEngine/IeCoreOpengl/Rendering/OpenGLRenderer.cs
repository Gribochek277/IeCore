using IeCoreInterfaces;
using IeCoreInterfaces.Core;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreOpengl.Extensions;
using IeCoreOpengl.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using System.Text;
using IeUtils;
using Microsoft.Extensions.Logging;
using IeCoreInterfaces.Assets;
using IeCoreEntites.Materials;
using OpenTK.Mathematics;

namespace IeCoreOpengl.Rendering
{
    public class OpenGLRenderer : IRenderer
    {
        private const string ModelObjectComponent = "ModelSceneObjectComponent";
        private const string MaterialObjectComponent = "MaterialSceneObjectComponent";
        private ISceneManager _sceneManager;
        private ISceneObjectComponent _materialObjectComponent;
        private ISceneObjectComponent _modelObjectComponent;
        private IUniformHelper _uniformHelper;
        private ILogger<OpenGLRenderer> _logger;
        private IAssetManager _assetManager;
        private Matrix4 _projection;
        private Matrix4 _view;
        private ISceneObjectComponent _camera;

        public OpenGLRenderer(ISceneManager sceneManager, IUniformHelper uniformHelper, IAssetManager assetManager, ILogger<OpenGLRenderer> logger)
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
            foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
            {
                //Generate textures
                foreach (Texture texture in _assetManager.RetrieveAll<Texture>())
                {
                   texture.Id = GL.GenTexture();
                }

                //Find model component in scene object.
                if (sceneObject.components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
                {
                    //Get model from scene object.
                    IModelComponent currentModelComponent = (IModelComponent)_modelObjectComponent;
                    //Generate VAO on OGL side and assign id of that buffer to model.
                    currentModelComponent.Model.VertexArrayObjectId = GL.GenVertexArray();
                    GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);
                    //Generate buffer on OGL side and assign id of that buffer to model.
                    currentModelComponent.Model.VertexBufferObjectId = GL.GenBuffer();
                    //Generate buffer on OGL side and assign id of that buffer to model.
                    currentModelComponent.Model.ElementBufferId = GL.GenBuffer();
                    //Bind created buffer to ArrayBuffer target.
                    GL.BindBuffer(BufferTarget.ArrayBuffer, currentModelComponent.Model.VertexBufferObjectId);
                    //Bind create buffer to elementBuffer.
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, currentModelComponent.Model.ElementBufferId);
                    //Get all vertices from model.
                    float[] VBOData = currentModelComponent.GetVBODataOfModel();
                    uint[] indices = currentModelComponent.GetIndicesOfModel();

                    //VBO Data consist of 5 digits(for now) 1st three - vertex positions, 4th and 5th item - texture coordinate
                    StringBuilder sb = new StringBuilder("Vertex coordinates: ");
                    sb.Append(currentModelComponent.Model.Name);
                    for(int i=0;  i < VBOData.Length;  i++)
                    {
                        if(i!=0 && (i % 4 == 0 || i % 3 == 0))
                        {
                            continue;
                        }
                        else
                        { 
                            sb.Append(" ");
                            sb.Append(VBOData[i]);
                        }
                    }
                    _logger.LogInformation(sb.ToString());
                    //Load model data to GPU.
                    GL.BufferData(BufferTarget.ArrayBuffer, VBOData.Length * sizeof(float), VBOData, BufferUsageHint.StaticDraw);
                    //Load indices data to GPU.
                    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
                }

                if (sceneObject.components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
                {
                    //Get material from scene object.
                    IMaterialComponent currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
                    Texture texture = currentMaterialComponent.materials.FirstOrDefault().Value.DiffuseTexture;

                    GL.BindTexture(TextureTarget.Texture2D, texture.Id);


                    GL.TexImage2D(TextureTarget.Texture2D,
                       0,
                       PixelInternalFormat.Srgb8,
                       (int)texture.TextureSize.X,
                       (int)texture.TextureSize.Y,
                       0,
                       PixelFormat.Bgra,
                       PixelType.UnsignedByte,
                       texture.Bytes);

                   

                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                    GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aPosition"), 
                        3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                    int texCoordLocation = currentMaterialComponent.ShaderProgram.GetAttributeAddress("aTexCoord");
                    GL.EnableVertexAttribArray(texCoordLocation);
                    GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                }

                _projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)_width / (float)_height, 0.1f, 120.0f);
            }
        }

        public void OnRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Back);

            foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
            {
                if(sceneObject.components.TryGetValue("Camera", out _camera))
                {
                    ICamera camera = (ICamera)_camera;
                    _view = camera.GetViewMatrix().ConvertToOpenTKMaxtrix4();
                }
                IMaterialComponent currentMaterialComponent = null;
                IModelComponent currentModelComponent = null;
                if (sceneObject.components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
                {
                    //Get material from scene object.
                    currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
                    currentMaterialComponent.ShaderProgram.EnableVertexAttribArrays();
                    Texture texture = currentMaterialComponent.materials.FirstOrDefault().Value.DiffuseTexture;

                    _uniformHelper.TryAddUniformTexture2D(texture.Id, "texture0", currentMaterialComponent.ShaderProgram, TextureUnit.Texture0);

                    currentMaterialComponent.ShaderProgram.UseProgram();

                    _uniformHelper.TryAddUniform(currentMaterialComponent.materials.FirstOrDefault().Value.DiffuseColor.ConvertToOpenTKVector(),
                        "Color",
                        currentMaterialComponent.ShaderProgram);
                }
               
                //Get all vertices from model.
                if (sceneObject.components.TryGetValue(ModelObjectComponent, out _modelObjectComponent))
                {
                    //Get model from scene object.
                    currentModelComponent = (IModelComponent)_modelObjectComponent;

                    Matrix4 modelMatrix = currentModelComponent.Model.Meshes.FirstOrDefault().Transform.ModelMatrix.ConvertToOpenTKMaxtrix4();
                    GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("model"), false, ref modelMatrix);
                    GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("projection"), false, ref _projection);
                    GL.UniformMatrix4(currentMaterialComponent.ShaderProgram.GetUniformAddress("view"), false, ref _view);

                    // Bind the VAO
                    GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);

                    GL.DrawElements(PrimitiveType.Triangles, currentModelComponent.GetIndicesOfModel().Length, DrawElementsType.UnsignedInt, 0);
                }

            }

        }

        public void OnResized()
        {
            GL.Viewport(0, 0, _width, _height);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        public void OnUpdated()
        {
            //TODO: Clean up all buffers.
            //throw new NotImplementedException();
        }

        public void SetViewPort(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
