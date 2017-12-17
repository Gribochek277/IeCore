using System;
using Irrational.Core.Renderer.Abstractions;
using OpenTK;
using System.Collections.Generic;
using Irrational.Shaders;
using Irrational.Utils;
using Irrational.Core.Entities;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Renderer
{
    public class OpenglRenderer : IRenderer
    {
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
        Dictionary<string, int> textures = new Dictionary<string, int>();
        Dictionary<string, ShaderProg> shaders = new Dictionary<string, ShaderProg>();
        Dictionary<String, Material> materials = new Dictionary<string, Material>();

        SceneObject light = null;

       // string activeShader = "default";

        float time = 0.0f;

        public void OnLoad()
        {
            lastMousePos = new Vector2(_gameWindow.Mouse.X, _gameWindow.Mouse.Y);

            GL.GenBuffers(1, out ibo_elements);

            // Load shaders from file
            //shaders.Add("default", new ShaderProg("vs.glsl", "fs.glsl", true));
            //shaders.Add("textured", new ShaderProg("vs_tex.glsl", "fs_tex.glsl", true));
            //shaders.Add("normal", new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true));

            //activeShader = "normal";

            light = new SceneObject() { ModelMesh = new Mesh() { Position = new Vector3(1, 1, 1) } };

            for (int i = 0; i < 1; i++) { 
            SceneObject sceneObject = new SceneObject() { MaterialSource = "Resources/Lion/Lion-snake.mtl" };
            sceneObject.OnLoad();
               
                sceneObject.shader = new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true);

                materials[sceneObject.materials.FirstOrDefault().Key] = sceneObject.materials.FirstOrDefault().Value;
                foreach(var texture in sceneObject.textures)
                textures[texture.Key] = texture.Value;

                // Create our objects
                WavefrontModelLoader modelLoader = new WavefrontModelLoader();
            sceneObject.ModelMesh = modelLoader.LoadFromFile("Resources/Lion/Lion-snake.obj");
            //sceneObject.ModelMesh.CalculateNormals();
            sceneObject.Position += new Vector3(0+(i*3), 0.0f-100, -10);
           // sceneObject.ModelMesh.TextureID = textures[materials["ZBrushPolyMesh3DSG"].DiffuseMap];
            sceneObject.Scale = new Vector3(1f, 1f, 1f)*0.2f;
            objects.Add(sceneObject);

           }

            for (int i = 0; i < 0; i++)
            {
                SceneObject sceneObject = new SceneObject() { MaterialSource = "Resources/knight3.mtl" };
                sceneObject.OnLoad();

                sceneObject.shader = new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true);

                materials[sceneObject.materials.FirstOrDefault().Key] = sceneObject.materials.FirstOrDefault().Value;
                textures[sceneObject.textures.FirstOrDefault().Key] = sceneObject.textures.FirstOrDefault().Value;

                // Create our objects
                WavefrontModelLoader modelLoader = new WavefrontModelLoader();
                sceneObject.ModelMesh = modelLoader.LoadFromFile("Resources/knight3.obj1");
                 sceneObject.ModelMesh.CalculateNormals();
                sceneObject.Position += new Vector3(0 + (i * 3), 0.0f, -10);
               // sceneObject.ModelMesh.TextureID = textures[materials["Knight"].DiffuseMap];
                //   sceneObject.Scale = new Vector3(1f, 1f, 1f);
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
                verts.AddRange(v.ModelMesh.GetVerts().ToList());
                inds.AddRange(v.ModelMesh.GetIndices(vertcount).ToList());
                colors.AddRange(v.ModelMesh.GetColorData().ToList());
                texcoords.AddRange(v.ModelMesh.GetTextureCoords());
                normals.AddRange(v.ModelMesh.GetNormals().ToList());
                vertcount += v.ModelMesh.VertCount;
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

                GL.BindBuffer(BufferTarget.ArrayBuffer, v.shader.GetBuffer("vPosition"));

                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(v.shader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

                // Buffer vertex color if shader supports it
                if (v.shader.GetAttribute("vColor") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, v.shader.GetBuffer("vColor"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(v.shader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }


                // Buffer texture coordinates if shader supports it
                if (v.shader.GetAttribute("texcoord") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, v.shader.GetBuffer("texcoord"));
                    GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(v.shader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
                }

                if (v.shader.GetAttribute("vNormal") != -1)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, v.shader.GetBuffer("vNormal"));
                    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(v.shader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
                }
                indiceat += v.ModelMesh.IndiceCount;

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

             
			int indiceat = 0;

           
            // Draw all our objects
            foreach(SceneObject v in objects)
            {
				GL.UseProgram(v.shader.ProgramID);
				v.shader.EnableVertexAttribArrays();
                
                int texId = textures[materials[v.materials.FirstOrDefault().Value.MaterialName].DiffuseMap];              
                int normId = textures[materials[v.materials.FirstOrDefault().Value.MaterialName].NormalMap];
               
               
                GL.UniformMatrix4(v.shader.GetUniform("model"), false, ref v.ModelMesh.ModelMatrix);
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
                GL.UniformMatrix4(v.shader.GetUniform("projection"), false, ref projection);
                Matrix4 view = cam.GetViewMatrix();
                GL.UniformMatrix4(v.shader.GetUniform("view"), false, ref view);

                if (v.shader.GetUniform("normaltexture") != -1)
                {
                    SetUniform("normaltexture", v.shader.ProgramID, 1);
                }

                if (v.shader.GetUniform("maintexture") != -1)
                {
                    SetUniform("maintexture", v.shader.ProgramID, 0);
                }


                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texId);
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, normId);               

                

                if (v.shader.GetUniform("lightColor") != -1)
                {
                    GL.Uniform3(v.shader.GetUniform("lightColor"), 1f, 1f, 1F);
                }

                if (v.shader.GetUniform("ambientStr") != -1)
                {
                    GL.Uniform1(v.shader.GetUniform("ambientStr"), 0.3f);
                }

                if (v.shader.GetUniform("specStr") != -1)
                {
                    //TODO : find a way how to extract specular exponent from material. Additional refactoring is requiered
                    GL.Uniform1(v.shader.GetUniform("specStr"), 10f);
                }

                if (v.shader.GetUniform("cameraPosition") != -1)
                {
                    GL.Uniform3(v.shader.GetUniform("cameraPosition"), cam.Position.X, cam.Position.Y, cam.Position.Z);
                }

                if (v.shader.GetUniform("lightPos") != -1)
                {
                    GL.Uniform3(v.shader.GetUniform("lightPos"), light.Position.X, light.Position.Y, light.Position.Z);
                }

                GL.DrawElements(BeginMode.Triangles, v.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.ModelMesh.IndiceCount;
          


                v.shader.DisableVertexAttribArrays();
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
                v.ModelMesh.CalculateModelMatrix();
                v.ModelMesh.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
                v.ModelMesh.ModelViewProjectionMatrix = v.ModelMesh.ModelMatrix * v.ModelMesh.ViewProjectionMatrix;
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

        public void SetUniform(string name,int programId, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(programId, name), value);
        }
    }
}
    