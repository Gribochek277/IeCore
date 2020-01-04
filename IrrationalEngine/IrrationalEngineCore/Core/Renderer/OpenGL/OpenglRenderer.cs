using System;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using OpenTK;
using System.Collections.Generic;
using IrrationalEngineCore.Core.Entities;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Renderer.OpenGL.Helpers;
using IrrationalEngineCore.Core.Shaders;
using OpenTK.Input;

namespace IrrationalEngineCore.Core.Renderer.OpenGL
{
    public class OpenglRenderer : IRenderer
    {
        private IUniformHelper _uniformHelper;
        private List<ISceneObject> _objects;
        private int windowWidth = 0;
        private int windowHeight = 0;

        private Vector3[] vertdata;
        private Vector3[] coldata;
        private Vector2[] texCoordinatesData;
        private Vector3[] normdata;

        private int[] indicedata;
        private int ibo_elements;
        private IPipelineData _pipelineData;

        public OpenglRenderer(IUniformHelper uniformHelper, IPipelineData pipelineData) {
            _uniformHelper = uniformHelper;
            _pipelineData = pipelineData;
        }

        public void OnLoad()
        {
            _pipelineData.OnLoad();
            _objects = _pipelineData.Objects;

            foreach (SceneObject v in _objects)
            {
                v.OnLoad();
            }

            GL.GenBuffers(1, out ibo_elements);
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            GL.Viewport(0, 0, windowWidth, windowHeight);

            _pipelineData.Cam.Position += new Vector3(0f, 0f, 3f);
            GL.ClearColor(0,0,1,1);
            GL.PointSize(5f);
            
            // Assemble vertex and index data for all volumes
            int vertcount = 0;

            List<Vector3> vertices = new List<Vector3>();
            List <int> indices = new List<int>();
            List <Vector3> colors = new List<Vector3>();
            List <Vector2> texcoords = new List<Vector2>();
            List <Vector3> normals = new List<Vector3>();

            foreach (SceneObject v in _objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];
                vertices.AddRange(meshComponent.ModelMesh.GetVerts().ToList());
                indices.AddRange(meshComponent.ModelMesh.GetIndices(vertcount).ToList());
                colors.AddRange(meshComponent.ModelMesh.GetColorData().ToList());
                texcoords.AddRange(meshComponent.ModelMesh.GetTextureCoords());
                normals.AddRange(meshComponent.ModelMesh.GetNormals().ToList());
                vertcount += meshComponent.ModelMesh.VertCount;
            }           

            vertdata = vertices.ToArray();
            indicedata = indices.ToArray();
            coldata = colors.ToArray();
            texCoordinatesData = texcoords.ToArray();
            normdata = normals.ToArray();
            

            Console.WriteLine("Vertexes: " + vertdata.Length);
            Console.WriteLine("Triangles: " + vertdata.Length/3);
            
            foreach (SceneObject v in _objects) {
                ISceneObjectComponent meshComponent;                
                v.components.TryGetValue("MeshSceneObjectComponent", out meshComponent);
                
                ISceneObjectComponent extractedMaterialComponent;
                if(v.components.TryGetValue("MaterialSceneObjectComponent", out extractedMaterialComponent)) {               
                    
                    MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)extractedMaterialComponent;            

                    GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.ShaderImplementation.shaderProg.GetBuffer("vPosition"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(materialComponent.ShaderImplementation.shaderProg.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

                    // Buffer vertex color if shader supports it. Currently commented out because not used for first implementation.
                  /*  if (materialComponent.ShaderImplementation.shaderProg.GetAttribute("vColor") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.ShaderImplementation.shaderProg.GetBuffer("vColor"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.ShaderImplementation.shaderProg.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }*/


                    // Buffer texture coordinates if shader supports it
                    if (materialComponent.ShaderImplementation.shaderProg.GetAttribute("texcoord") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.ShaderImplementation.shaderProg.GetBuffer("texcoord"));
                        GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texCoordinatesData.Length * Vector2.SizeInBytes), texCoordinatesData, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.ShaderImplementation.shaderProg.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                    }

                    if (materialComponent.ShaderImplementation.shaderProg.GetAttribute("vNormal") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.ShaderImplementation.shaderProg.GetBuffer("vNormal"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.ShaderImplementation.shaderProg.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }
                    

                }
            }
        }

        public void OnRendered()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.FramebufferSrgb);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            Matrix4 projection =
             Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)windowWidth / (float)windowHeight, 0.1f, 120.0f);
            Matrix4 view = _pipelineData.Cam.GetViewMatrix();
            // Update object positions
           

            int indiceat = 36; //start rendering after skybox

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Draw all our objects
            for (int i=1; i<_objects.Count;i++)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)_objects[i].components["MeshSceneObjectComponent"];
                MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)_objects[i].components["MaterialSceneObjectComponent"];

               
                GL.UseProgram(materialComponent.ShaderImplementation.shaderProg.ProgramID);
                materialComponent.ShaderImplementation.shaderProg.EnableVertexAttribArrays();
               
                Matrix4 modelMatrix = meshComponent.ModelMesh.Transform.ModelMatrix;
                GL.UniformMatrix4(materialComponent.ShaderImplementation.shaderProg.GetUniform("model"), false, ref modelMatrix);
                GL.UniformMatrix4(materialComponent.ShaderImplementation.shaderProg.GetUniform("projection"), false, ref projection);
                GL.UniformMatrix4(materialComponent.ShaderImplementation.shaderProg.GetUniform("view"), false, ref view);//think about adding to uniform helper Matrix4 type, but for now it's not required.

               /* _uniformHelper.TryAddUniformTexture2D(texId, "maintexture", materialComponent.Shader, TextureUnit.Texture0);
                _uniformHelper.TryAddUniformTexture2D(normId, "normaltexture", materialComponent.Shader, TextureUnit.Texture1);
                _uniformHelper.TryAddUniformTexture2D(metRoughId, "metallicroughness", materialComponent.Shader, TextureUnit.Texture2);
                _uniformHelper.TryAddUniformTexture2D(ambientId, "defaultAO", materialComponent.Shader, TextureUnit.Texture3);
                W

                //TODO retrieve it properly skybox texture
                _uniformHelper.TryAddUniformTextureCubemap(3, "irradianceMap", materialComponent.Shader, TextureUnit.Texture4);
                _uniformHelper.TryAddUniformTextureCubemap(4, "prefilterMap", materialComponent.Shader, TextureUnit.Texture5);
                _uniformHelper.TryAddUniformTexture2D(5,"brdfLUT", materialComponent.Shader, TextureUnit.Texture6);
                _uniformHelper.TryAddUniform1(1f, "ambientStr", shaderProg);
                */
               materialComponent.ShaderImplementation.SetSpecificUniforms(_pipelineData);

                //_uniformHelper.TryAddUniform1(1f, "specStr", materialComponent.ShaderImplementation.shaderProg);//TODO : find a way how to extract specular exponent from material. Additional refactoring is required.
                //PBR uniforms
                /*_uniformHelper.TryAddUniform1((float)Math.Abs(Math.Sin(time*0.1f)), "metallic", materialComponent.ShaderImplementation.shaderProg);
                _uniformHelper.TryAddUniform1((float)Math.Abs(Math.Cos(time*0.1f)), "roughness", materialComponent.ShaderImplementation.shaderProg);*/

                GL.DrawElements(BeginMode.Triangles, meshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += meshComponent.ModelMesh.IndiceCount;

                materialComponent.ShaderImplementation.shaderProg.DisableVertexAttribArrays();
            }
            GL.Disable(EnableCap.FramebufferSrgb);

            SkyboxRenderHelper.RenderSkybox(view, projection, _pipelineData.Skybox);
           
            GL.Flush();
        }

        public void OnResized()
        {
            //throw new NotImplementedException("Fix resize");
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }

        public void OnUpdated()
        {
            // Update model view matrices
            foreach (SceneObject v in _objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];

               // meshComponent.ModelMesh.Transform.CalculateModelMatrix();
               // meshComponent.ModelMesh.Transform.ViewProjectionMatrix = 
              //  pipelineData.Cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)_gameWindow.ClientSize.Width 
               // / (float)_gameWindow.ClientSize.Height, 1.0f, 120);

               // meshComponent.ModelMesh.Transform.ModelViewProjectionMatrix = 
               // meshComponent.ModelMesh.Transform.ModelMatrix * meshComponent.ModelMesh.Transform.ViewProjectionMatrix;
            }
        }

        public void SetViewPort(int width, int height)
        {
            windowWidth = width;
            windowHeight = height;
            GL.Viewport(0,0, width, height);
        }
    }
}
    