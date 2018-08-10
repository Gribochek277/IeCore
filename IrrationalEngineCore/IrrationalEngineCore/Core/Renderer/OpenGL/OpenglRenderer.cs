using System;
using Irrational.Core.Renderer.Abstractions;
using OpenTK;
using System.Collections.Generic;
using Irrational.Core.Entities;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Entities.SceneObjectComponents;
using Irrational.Core.Renderer.OpenGL.Helpers;

namespace Irrational.Core.Renderer.OpenGL
{
    public class OpenglRenderer : IRenderer
    {
        private UniformHelper _uniformHelper;
        private GameWindow _gameWindow;
        private List<ISceneObject> _objects;
        private Camera _cam;
        private SceneObject skybox = null;

        public OpenglRenderer(GameWindow gameWindow) {
            _gameWindow = gameWindow;
            _uniformHelper = new UniformHelper();
        }
                
        Vector3[] vertdata;
        Vector3[] coldata;
        Vector2[] texcoorddata;
        Vector3[] normdata;

        int[] indicedata;
        int ibo_elements;
           
        List<SceneObject> lights = new List<SceneObject>();

        float time = 0.0f;

        public void OnLoad(List<ISceneObject> sceneObjects)
        {
            _objects = sceneObjects;

            //sceneObjects which required for default render pipeline camera etc.
            List<SceneObject> specificSceneObjects = new List<SceneObject>();
            //gather specific objects
            foreach (SceneObject sceneObject in sceneObjects)
            {
                //gather all light sources
                if (sceneObject.components.ContainsKey("PointLightSceneObjectComponent"))
                {
                    lights.Add(sceneObject);
                }
                //gather cemeras
                if (sceneObject.GetType().Name == "PlayerCamera")
                {
                    _cam = (Camera)sceneObject.components["Camera"];
                    specificSceneObjects.Add(sceneObject);
                }
                //gather skybox
                if (sceneObject.GetType().Name == "Skybox")
                {
                    skybox = sceneObject;                
                }
            }

            //remove specific objects from main collection of sceneObjects 
            foreach (SceneObject sceneObject in specificSceneObjects)
            {
                sceneObject.OnLoad();
                _objects.Remove(sceneObject);
            }

            foreach (SceneObject sceneObject in lights)
            {
                _objects.Remove(sceneObject);
            }

            foreach (SceneObject v in _objects)
            {
                v.OnLoad();
            }

            GL.GenBuffers(1, out ibo_elements);

            _cam.Position += new Vector3(0f, 0f, 3f);
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
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];
                try {     //this code is bullshit. need to do something with it.                
                    MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)v.components["MaterialSceneObjectComponent"];
                                    

                    GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.Shader.GetBuffer("vPosition"));

                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(materialComponent.Shader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

                    // Buffer vertex color if shader supports it
                    if (materialComponent.Shader.GetAttribute("vColor") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.Shader.GetBuffer("vColor"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.Shader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }


                    // Buffer texture coordinates if shader supports it
                    if (materialComponent.Shader.GetAttribute("texcoord") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.Shader.GetBuffer("texcoord"));
                        GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.Shader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                    }

                    if (materialComponent.Shader.GetAttribute("vNormal") != -1)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, materialComponent.Shader.GetBuffer("vNormal"));
                        GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                        GL.VertexAttribPointer(materialComponent.Shader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
                    }
                    

                } catch {
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
            Matrix4 view = _cam.GetViewMatrix();
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

                int texId = materialComponent.Textures[materialComponent.Materials.FirstOrDefault().Value.DiffuseMap];
                int normId = materialComponent.Textures[materialComponent.Materials.FirstOrDefault().Value.NormalMap];
                GL.UseProgram(materialComponent.Shader.ProgramID);
                materialComponent.Shader.EnableVertexAttribArrays();
               
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("model"), false, ref meshComponent.ModelMesh.ModelMatrix);
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("projection"), false, ref projection);
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("view"), false, ref view);//think about adding to uniformhelper Matrix4 type, but for now it's not required.

                _uniformHelper.TryAddUniformTexture2D(texId, "maintexture", materialComponent.Shader, TextureUnit.Texture0);
                _uniformHelper.TryAddUniformTexture2D(normId, "normaltexture", materialComponent.Shader, TextureUnit.Texture1);

                //TODO retrieve it properly skybox texture
                _uniformHelper.TryAddUniformTextureCubemap(3, "irradianceMap", materialComponent.Shader, TextureUnit.Texture2);
                _uniformHelper.TryAddUniformTextureCubemap(4, "prefilterMap", materialComponent.Shader, TextureUnit.Texture3);
                _uniformHelper.TryAddUniformTexture2D(5,"brdfLUT", materialComponent.Shader, TextureUnit.Texture4);

                _uniformHelper.TryAddUniform1(lights.Count(), "numberOfLights", materialComponent.Shader);
                Vector3[] lightpositions = new Vector3[lights.Count()];
                Vector3[] lightcolors = new Vector3[lights.Count()];
                for (int j = 0; j < lights.Count; ++j) {
                    var lightComponent =  (PointLightSceneObjectComponent)lights[j].components["PointLightSceneObjectComponent"];
                    lightcolors[j] = lightComponent.Color * lightComponent.LightStrenght;
                    lightpositions[j] = lights[j].Position;
                }

                bool suc = _uniformHelper.TryAddUniform1(lightcolors, "lightColor[0]", materialComponent.Shader);
                bool suc2 = _uniformHelper.TryAddUniform1(lightpositions, "lightPos[0]", materialComponent.Shader);

                _uniformHelper.TryAddUniform1(1f, "ambientStr", materialComponent.Shader);
                _uniformHelper.TryAddUniform1(1f, "specStr", materialComponent.Shader);//TODO : find a way how to extract specular exponent from material. Additional refactoring is requiered.
                _uniformHelper.TryAddUniform1(_cam.Position, "cameraPosition", materialComponent.Shader);
                //PBR uniforms
                _uniformHelper.TryAddUniform1((float)Math.Abs(Math.Sin(time*0.1f)), "metallic", materialComponent.Shader);
                _uniformHelper.TryAddUniform1((float)Math.Abs(Math.Cos(time*0.1f)), "roughness", materialComponent.Shader);
               
                GL.DrawElements(BeginMode.Triangles, meshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += meshComponent.ModelMesh.IndiceCount;

                materialComponent.Shader.DisableVertexAttribArrays();
            }

            indiceat += SkyboxRenderHelper.RenderHdrToCubemapSkybox(view, projection, skybox);
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

                meshComponent.ModelMesh.CalculateModelMatrix();
                meshComponent.ModelMesh.ViewProjectionMatrix = _cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40);
                meshComponent.ModelMesh.ModelViewProjectionMatrix = meshComponent.ModelMesh.ModelMatrix * meshComponent.ModelMesh.ViewProjectionMatrix;
            }
        }        
    }
}
    