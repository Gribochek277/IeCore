using IeCoreInterfaces;
using IeCoreInterfaces.Core;
using IeCoreInterfaces.Rendering;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreOpengl.Extensions;
using IeCoreOpengl.Helpers;
using IeCoreOpengl.Shaders;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using System;
using System.Linq;
using System.Text;

namespace IeCoreOpengl.Rendering
{
    public class OpenGLRenderer : IRenderer
    {
        private ISceneManager _sceneManager;
        private ISceneObjectComponent _materialObjectComponent;
        private ISceneObjectComponent _modelObjectComponent;

        public OpenGLRenderer(ISceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        private int _width, _height;
        public void OnLoad()
        {           
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
            {
                //Find model component in scene object.
                if (sceneObject.components.TryGetValue("ModelSceneObjectComponent", out _modelObjectComponent))
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
                    Console.WriteLine(sb.ToString());
                    //Load model data to GPU.
                    GL.BufferData(BufferTarget.ArrayBuffer, VBOData.Length * sizeof(float), VBOData, BufferUsageHint.StaticDraw);
                    //Load indices data to GPU.
                    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
                }

                if (sceneObject.components.TryGetValue("MaterialSceneObjectComponent", out _materialObjectComponent))
                {
                    //Get material from scene object.
                    IMaterialComponent currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
                    GL.VertexAttribPointer(currentMaterialComponent.ShaderProgram.GetAttributeAddress("aPosition"), 
                        3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);     
                    
                }
            }
        }

        public void OnRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (ISceneObject sceneObject in _sceneManager.Scene.SceneObjects)
            {
                if (sceneObject.components.TryGetValue("MaterialSceneObjectComponent", out _materialObjectComponent))
                {
                    //Get material from scene object.
                    IMaterialComponent currentMaterialComponent = (IMaterialComponent)_materialObjectComponent;
                    currentMaterialComponent.ShaderProgram.EnableVertexAttribArrays();
                    currentMaterialComponent.ShaderProgram.UseProgram();

                    UniformHelper.TryAddUniform(currentMaterialComponent.materials.FirstOrDefault().Value.DiffuseColor.ConvertToOpenTKVector(),
                        "Color",
                        currentMaterialComponent.ShaderProgram);
                }
               
                //Get all vertices from model.
                if (sceneObject.components.TryGetValue("ModelSceneObjectComponent", out _modelObjectComponent))
                {                   
                    //Get model from scene object.
                    IModelComponent currentModelComponent = (IModelComponent)_modelObjectComponent;

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
