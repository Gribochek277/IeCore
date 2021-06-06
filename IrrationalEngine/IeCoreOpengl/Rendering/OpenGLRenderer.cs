using System;
using System.Linq;
using System.Text;
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
                    GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);
                    //Generate buffer on OGL side and assign id of that buffer to model.
                    currentModelComponent.Model.VertexBufferObjectId = GL.GenBuffer();
                    //Generate buffer on OGL side and assign id of that buffer to model.
                    currentModelComponent.Model.ElementBufferId = GL.GenBuffer();
                    
                    //Get all vertices from model.
                    float[] vboPositionData = currentModelComponent.GetVboPositionDataOfModel();
                    uint[] indices = currentModelComponent.GetIndicesOfModel();

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
                    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

                    if (sceneObject.Components.TryGetValue(AnimationSceneObjectComponent, out _animationObjectComponent))
                    {
                        var currentAnimationComponent = (IAnimationComponent)_animationObjectComponent;
                        var weights = currentModelComponent.Model.Meshes[0].Skeleton.Bones.Select(x => x.VertexWeights).ToArray();
                        var offsetmatrix = currentModelComponent.Model.Meshes[0].Skeleton.Bones.Select(x => x.OffsetMatrix).ToArray();
                    }

                    if (sceneObject.Components.TryGetValue(MaterialObjectComponent, out _materialObjectComponent))
                    {
                        //Get material from scene object.
                        var currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
                        Texture texture = currentMaterialComponent.Materials.FirstOrDefault().Value.DiffuseTexture;

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
                if(sceneObject.Components.TryGetValue("Camera", out _camera))
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

                    GL.BindVertexArray(currentModelComponent.Model.VertexArrayObjectId);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, currentModelComponent.Model.ElementBufferId);


                    Matrix4 modelMatrix = currentModelComponent.Model.Meshes.FirstOrDefault().Transform.ModelMatrix.ConvertToOpenTkMatrix4();
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
