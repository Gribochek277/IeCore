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

        string activeShader = "default";

        float time = 0.0f;

        public void OnLoad()
        {
            lastMousePos = new Vector2(_gameWindow.Mouse.X, _gameWindow.Mouse.Y);

            GL.GenBuffers(1, out ibo_elements);

            // Load shaders from file
            shaders.Add("default", new ShaderProg("vs.glsl", "fs.glsl", true));
            shaders.Add("textured", new ShaderProg("vs_tex.glsl", "fs_tex.glsl", true));
            shaders.Add("normal", new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true));

            activeShader = "textured";

            for(int i = 0; i < 300; i++) { 
            SceneObject sceneObject = new SceneObject() { MaterialSource = "Resources/knight3.mtl" };
            sceneObject.OnLoad();
            materials = sceneObject.materials;
            textures = sceneObject.textures;

            // Create our objects
            WavefrontModelLoader modelLoader = new WavefrontModelLoader();
            sceneObject.ModelMesh = modelLoader.LoadFromFile("Resources/knight3.obj1");
            sceneObject.Position += new Vector3(0+(i*3), 0.0f, 0);
            sceneObject.ModelMesh.TextureID = textures[materials["Knight"].DiffuseMap];
            sceneObject.Scale = new Vector3(0.3f, 0.3f, 0.3f);
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
        }

        public void OnRendered()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (shaders[activeShader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shaders[activeShader].GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Update object positions
            time += (float)this._gameWindow.UpdatePeriod;




            GL.UseProgram(shaders[activeShader].ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            shaders[activeShader].EnableVertexAttribArrays();
            

            int indiceat = 0;

            // Draw all our objects
            foreach(SceneObject v in objects)
            {
                GL.BindTexture(TextureTarget.Texture2D, v.ModelMesh.TextureID);
                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelMesh.ModelViewProjectionMatrix);

                if (shaders[activeShader].GetAttribute("maintexture") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetAttribute("maintexture"), v.ModelMesh.TextureID);
                }

                GL.DrawElements(BeginMode.Triangles, v.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.ModelMesh.IndiceCount;
            }

            shaders[activeShader].DisableVertexAttribArrays();

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
            for(int i=0;i<objects.Count;i++)
            {
                objects[i].Position = new Vector3(-(objects.Count/2) +i + ((float)Math.Sin(time) * 2), -0.5f + (float)Math.Sin(time), -3.0f);
                objects[i].Rotation = new Vector3(0.55f * time, 0.25f * time, 0);
            }


            // Update model view matrices
            foreach (SceneObject v in objects)
            {
                v.ModelMesh.CalculateModelMatrix();
                v.ModelMesh.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, _gameWindow.ClientSize.Width / (float)_gameWindow.ClientSize.Height, 1.0f, 40.0f);
                v.ModelMesh.ModelViewProjectionMatrix = v.ModelMesh.ModelMatrix * v.ModelMesh.ViewProjectionMatrix;
            }


            // Reset mouse position
            if (_gameWindow.Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                lastMousePos += delta;

                cam.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }
        }

        /// <summary>
        /// Moves the mouse cursor to the center of the screen
        /// </summary>
        void ResetCursor()
        {
            // OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
    }
}
    