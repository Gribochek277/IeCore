using System;
using Irrational.Core.Renderer.Abstractions;
using OpenTK;
using System.Collections.Generic;
using Irrational.Shaders;
using Irrational.Core.Entities;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Renderer.OpenGL
{
    public class OpenglRenderer : IRenderer
    {
        private UniformHelper _uniformHelper;

        public OpenglRenderer() {
            _uniformHelper = new UniformHelper();
        }

        public GameWindow _gameWindow { get; set; }
        Vector3[] vertdata;
        Vector3[] coldata;
        Vector2[] texcoorddata;
        Vector3[] normdata;

        int[] indicedata;
        int ibo_elements;
        Camera cam = new Camera();
        Vector2 lastMousePos = new Vector2();

        List<SceneObject> objects = new List<SceneObject>();

        MeshSceneObjectComponent light = null;

       // string activeShader = "default";

        float time = 0.0f;

        public void OnLoad()
        {
            lastMousePos = new Vector2(_gameWindow.Mouse.X, _gameWindow.Mouse.Y);

            GL.GenBuffers(1, out ibo_elements);

            light = new MeshSceneObjectComponent()
            {
                ModelMesh = new Mesh()
                {
                    Position = new Vector3(0, 3, 3)
                }
            };
            

            for (int i = 0; i < 1; i++) {
                MaterialSceneObjectComponent material = new MaterialSceneObjectComponent()
                {
                    MaterialSource = "Resources/Lion/Lion-snake.mtl",
                    Shader = new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true)
                };
                
                MeshSceneObjectComponent meshComponent = new MeshSceneObjectComponent(
                    new WavefrontModelLoader(),
                    "Resources/Lion/Lion-snake.obj"
                    );

                SceneObject sceneObject = new SceneObject();
                sceneObject.AddComponent(material);
                sceneObject.AddComponent(meshComponent);
                sceneObject.OnLoad();
                //sceneObject.ModelMesh.CalculateNormals();
                sceneObject.Position += new Vector3(0+(i*3), 0.0f-100, -10);
                sceneObject.Scale = new Vector3(1f, 1f, 1f)*0.2f;
                objects.Add(sceneObject);

           }

            cam.Position += new Vector3(0f, 0f, 3f);
            GL.ClearColor(Color.Black);
            GL.PointSize(5f);
            
            // Assemble vertex and indice data for all volumes
            int vertcount = 0;

            List<Vector3> verts = new List<Vector3>();
            List <int> inds = new List<int>();
            List <Vector3> colors = new List<Vector3>();
            List <Vector2> texcoords = new List<Vector2>();
            List <Vector3> normals = new List<Vector3>();

            foreach (SceneObject v in objects)
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

            int indiceat = 0;
            foreach (SceneObject v in objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];
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

                indiceat += meshComponent.ModelMesh.IndiceCount;
            }
        }

        public void OnRendered()
        {
          
            // Update object positions
            time += (float)this._gameWindow.RenderPeriod;
           
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.FramebufferSrgb);

            int indiceat = 0;
           
            // Draw all our objects
            foreach(SceneObject v in objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];
                MaterialSceneObjectComponent materialComponent = (MaterialSceneObjectComponent)v.components["MaterialSceneObjectComponent"];

                int texId = materialComponent.textures[materialComponent.materials.FirstOrDefault().Value.DiffuseMap];
                int normId = materialComponent.textures[materialComponent.materials.FirstOrDefault().Value.NormalMap];
                GL.UseProgram(materialComponent.Shader.ProgramID);
                materialComponent.Shader.EnableVertexAttribArrays();
               
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("model"), false, ref meshComponent.ModelMesh.ModelMatrix);
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("projection"), false, ref projection);
                Matrix4 view = cam.GetViewMatrix();
                GL.UniformMatrix4(materialComponent.Shader.GetUniform("view"), false, ref view);//think about adding to uniformhelper Matrix4 type, but for now it's not required.

                _uniformHelper.TryAddUniformTexture2D(texId, "maintexture", materialComponent.Shader, TextureUnit.Texture0);
                _uniformHelper.TryAddUniformTexture2D(normId, "normaltexture", materialComponent.Shader, TextureUnit.Texture1);
                _uniformHelper.TryAddUniform1(new Vector3(1f, 1f, 1f), "lightColor", materialComponent.Shader);
                _uniformHelper.TryAddUniform1(0.1f, "ambientStr", materialComponent.Shader);
                _uniformHelper.TryAddUniform1(1f, "specStr", materialComponent.Shader);//TODO : find a way how to extract specular exponent from material. Additional refactoring is requiered.
                _uniformHelper.TryAddUniform1(cam.Position, "cameraPosition", materialComponent.Shader);
                _uniformHelper.TryAddUniform1(new Vector3(0f,0f,3f), "lightPos", materialComponent.Shader);
                GL.Enable(EnableCap.FramebufferSrgb);
                GL.DrawElements(BeginMode.Triangles, meshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += meshComponent.ModelMesh.IndiceCount;

                materialComponent.Shader.DisableVertexAttribArrays();
            }

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

        public void OnUpdated(double deltatime)
        {
            for(int i=0;i<objects.Count;i++)
            {
				objects[i].Position = new Vector3(-(objects.Count) + i*3.5f, 0-2.5f, -5.0f);
                objects[i].Rotation = new Vector3(0, 0.25f * time, 0);
            }

            // Update model view matrices
            foreach (SceneObject v in objects)
            {
                MeshSceneObjectComponent meshComponent = (MeshSceneObjectComponent)v.components["MeshSceneObjectComponent"];

                meshComponent.ModelMesh.CalculateModelMatrix();
                meshComponent.ModelMesh.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
                meshComponent.ModelMesh.ModelViewProjectionMatrix = meshComponent.ModelMesh.ModelMatrix * meshComponent.ModelMesh.ViewProjectionMatrix;
            }

            light.ModelMesh.CalculateModelMatrix();
            light.ModelMesh.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
            light.ModelMesh.ModelViewProjectionMatrix = light.ModelMesh.ModelMatrix * light.ModelMesh.ViewProjectionMatrix;

            // Reset mouse position
            if (_gameWindow.Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                lastMousePos += delta;

              //  cam.AddRotation(delta.X, delta.Y);
                ResetCursor((float)deltatime);
            }
        }

        /// <summary>
        /// Moves the mouse cursor to the center of the screen
        /// </summary>
        void ResetCursor(float deltatime)
        {
            // OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
    }
}
    