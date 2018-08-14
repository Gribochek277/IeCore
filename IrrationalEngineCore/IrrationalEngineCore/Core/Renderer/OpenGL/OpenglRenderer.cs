using System;
using Irrational.Core.Renderer.Abstractions;
using OpenTK;
using System.Collections.Generic;
using Irrational.Core.Entities;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.SceneObjectComponents;
using Irrational.Core.Renderer.OpenGL.Helpers;

namespace Irrational.Core.Renderer.OpenGL
{
    public class OpenglRenderer : IRenderer
    {
        private UniformHelper _uniformHelper;
        private GameWindow _gameWindow;
        private List<ISceneObject> _objects;
                
        private Vector3[] vertdata;
        private Vector3[] coldata;
        private Vector2[] texcoorddata;
        private Vector3[] normdata;

        private int[] indicedata;
        private int ibo_elements;
           

        private float time = 0.0f;

        private PipelineData pipelineData;


        public OpenglRenderer(GameWindow gameWindow) {
            _gameWindow = gameWindow;
            _uniformHelper = new UniformHelper();
        }

        public void OnLoad(List<ISceneObject> sceneObjects)
        {
            pipelineData = new PipelineData(sceneObjects);
            pipelineData.OnLoad();
            _objects = pipelineData.Objects;

            foreach (SceneObject v in _objects)
            {
                v.OnLoad();
            }

            GL.GenBuffers(1, out ibo_elements);

            pipelineData.Cam.Position += new Vector3(0f, 0f, 3f);
            GL.ClearColor(0,0,1,1);
            GL.PointSize(5f);
            
            // Assemble vertex and indice data for all volumes
            int vertcount = 0;

            List<Vector3> verts = new List<Vector3>();
            List <int> inds = new List<int>();
            List <Vector3> colors = new List<Vector3>();
            List <Vector2> texcoords = new List<Vector2>();
            List <Vector3> normals = new List<Vector3>();

            foreach (SceneObject v in _objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];
                verts.AddRange(meshComponent.ModelMesh.GetVerts().ToList());
                inds.AddRange(meshComponent.ModelMesh.GetIndices(vertcount).ToList());
                colors.AddRange(meshComponent.ModelMesh.GetColorData().ToList());
                texcoords.AddRange(meshComponent.ModelMesh.GetTextureCoords());
                normals.AddRange(meshComponent.ModelMesh.GetNormals().ToList());
                vertcount += meshComponent.ModelMesh.VertCount;
            }           

            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            texcoorddata = texcoords.ToArray();
            normdata = normals.ToArray();
            

            Console.WriteLine("Vertexies: " + vertdata.Length);
            Console.WriteLine("Triangles: " + vertdata.Length/3);
            
            foreach (SceneObject v in _objects) {
                ISceneObjectComponent meshComponent;                
                v.components.TryGetValue("MeshSceneObjectComponent", out meshComponent);
                
                ISceneObjectComponent extractedMaterialComponent;
                if(v.components.TryGetValue("MaterialSceneObjectComponent", out extractedMaterialComponent)) {               
                    
                    MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)extractedMaterialComponent;            

                    GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.shaderImplementation.shaderProg.GetBuffer("vPosition"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(materialComponent.shaderImplementation.shaderProg.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

                    // Buffer vertex color if shader supports it. Currently commented out vecause not used for first implementation.
                  /*  if (materialComponent.shaderImplementation.shaderProg.GetAttribute("vColor") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.shaderImplementation.shaderProg.GetBuffer("vColor"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.shaderImplementation.shaderProg.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }*/


                    // Buffer texture coordinates if shader supports it
                    if (materialComponent.shaderImplementation.shaderProg.GetAttribute("texcoord") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.shaderImplementation.shaderProg.GetBuffer("texcoord"));
                        GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.shaderImplementation.shaderProg.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                    }

                    if (materialComponent.shaderImplementation.shaderProg.GetAttribute("vNormal") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.shaderImplementation.shaderProg.GetBuffer("vNormal"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.shaderImplementation.shaderProg.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }
                    

                }
            }
        }

        public void OnRendered()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.FramebufferSrgb);
            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
            Matrix4 view = pipelineData.Cam.GetViewMatrix();
            // Update object positions
            time += (float)this._gameWindow.RenderPeriod;

            this._gameWindow.Title = "FPS: " + (1f / this._gameWindow.RenderPeriod).ToString("0.");

            int indiceat = 36; //start rendering after skybox

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Draw all our objects
            for (int i=1; i<_objects.Count;i++)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)_objects[i].components["MeshSceneObjectComponent"];
                MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)_objects[i].components["MaterialSceneObjectComponent"];

               
                GL.UseProgram(materialComponent.shaderImplementation.shaderProg.ProgramID);
                materialComponent.shaderImplementation.shaderProg.EnableVertexAttribArrays();
               
                Matrix4 modelMatrix = meshComponent.ModelMesh.Transform.ModelMatrix;
                GL.UniformMatrix4(materialComponent.shaderImplementation.shaderProg.GetUniform("model"), false, ref modelMatrix);
                GL.UniformMatrix4(materialComponent.shaderImplementation.shaderProg.GetUniform("projection"), false, ref projection);
                GL.UniformMatrix4(materialComponent.shaderImplementation.shaderProg.GetUniform("view"), false, ref view);//think about adding to uniformhelper Matrix4 type, but for now it's not required.

               /* _uniformHelper.TryAddUniformTexture2D(texId, "maintexture", materialComponent.Shader, TextureUnit.Texture0);
                _uniformHelper.TryAddUniformTexture2D(normId, "normaltexture", materialComponent.Shader, TextureUnit.Texture1);
                _uniformHelper.TryAddUniformTexture2D(metRoughId, "metallicroughness", materialComponent.Shader, TextureUnit.Texture2);
                _uniformHelper.TryAddUniformTexture2D(ambientId, "defaultAO", materialComponent.Shader, TextureUnit.Texture3);


                //TODO retrieve it properly skybox texture
                _uniformHelper.TryAddUniformTextureCubemap(3, "irradianceMap", materialComponent.Shader, TextureUnit.Texture4);
                _uniformHelper.TryAddUniformTextureCubemap(4, "prefilterMap", materialComponent.Shader, TextureUnit.Texture5);
                _uniformHelper.TryAddUniformTexture2D(5,"brdfLUT", materialComponent.Shader, TextureUnit.Texture6);
                */

                materialComponent.shaderImplementation.SetSpecificUniforms(pipelineData);              

                //_uniformHelper.TryAddUniform1(1f, "specStr", materialComponent.shaderImplementation.shaderProg);//TODO : find a way how to extract specular exponent from material. Additional refactoring is requiered.
                //PBR uniforms
                /*_uniformHelper.TryAddUniform1((float)Math.Abs(Math.Sin(time*0.1f)), "metallic", materialComponent.shaderImplementation.shaderProg);
                _uniformHelper.TryAddUniform1((float)Math.Abs(Math.Cos(time*0.1f)), "roughness", materialComponent.shaderImplementation.shaderProg);*/
               
                GL.DrawElements(BeginMode.Triangles, meshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += meshComponent.ModelMesh.IndiceCount;

                materialComponent.shaderImplementation.shaderProg.DisableVertexAttribArrays();
            }

            indiceat += SkyboxRenderHelper.RenderHdrToCubemapSkybox(view, projection, pipelineData.Skybox);
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);
            GL.Flush();
            _gameWindow.SwapBuffers();
        }

        public void OnResized()
        {
            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);
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

                meshComponent.ModelMesh.Transform.CalculateModelMatrix();
                meshComponent.ModelMesh.Transform.ViewProjectionMatrix = 
                pipelineData.Cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width 
                / (float)_gameWindow.ClientSize.Height, 1.0f, 40);

                meshComponent.ModelMesh.Transform.ModelViewProjectionMatrix = 
                meshComponent.ModelMesh.Transform.ModelMatrix * meshComponent.ModelMesh.Transform.ViewProjectionMatrix;
            }
        }        
    }
}
    