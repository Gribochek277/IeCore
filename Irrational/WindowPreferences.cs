using System;
using OpenGL;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Input;

namespace IrrationalSpace
{
    public class WindowPreferences : GameWindow
    {
        public static int widght = 800, height = 600;


        private static float xangle, yangle;
        public static bool autorotate = false;

        private static bool enableLight = true;

        private static List<Star> stars = new List<Star>();
        private static Random generator = new Random(Environment.TickCount);
        private static List<SceneObject> objectsOnScene = new List<SceneObject>();

        private static int currentObject = 0;

        public static float lightStr = 1f;
        public static bool fullscreen = false;
        public static bool alphaBlending = false;
        public static float alphaStr = 2f;

        public WindowPreferences() : base(1280, 720, GraphicsMode.Default, "Irrational engine",
            GameWindowFlags.Default, DisplayDevice.Default,
            // ask for an OpenGL 3.0 forward compatible context
            3, 0, GraphicsContextFlags.ForwardCompatible)
        { }

        protected override void OnLoad(EventArgs e)
        {
            widght = 800;
            height = 600;

            Gl.Enable(EnableCap.DepthTest);
            Gl.Disable(EnableCap.Blend);
            SceneObject sceneObject = new SceneObject("resources/h.obj", new OpenGL.Vector3(-0.3f, -1.2f, 1), new OpenGL.Vector3(1, 1, 1) * 0.01f, new OpenGL.Vector3(1, 1, 1));
            sceneObject.SetMAterial("resources/h.jpg", "resources/hbump.jpg", true, new OpenGL.Vector3(0, 0, 1), lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);





            objectsOnScene.Add(sceneObject);

            Gl.BlendFunc(BlendingFactorSrc.OneMinusConstantAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }
        
        protected override void OnResize(EventArgs e)
        {
            WindowPreferences.widght = this.Width;
            WindowPreferences.height = this.Height;
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                Gl.Viewport(0, 0, WindowPreferences.widght, WindowPreferences.height);
                objectsOnScene[i].program.Use();
                objectsOnScene[i].program["projection_matrix"].SetValue(OpenGL.Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)WindowPreferences.widght / WindowPreferences.height, 0.1f, 1000f));
            }
        }

        private void OnClose()
        {
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                objectsOnScene[i].program.DisposeChildren = true;
                objectsOnScene[i].program.Dispose();
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // this is called every frame, put game logic here
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
        


            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                objectsOnScene[i].program["enable_lighting"].SetValue(enableLight);
                objectsOnScene[i].program["light_strenght"].SetValue(lightStr);
                objectsOnScene[i].program["alpha_str"].SetValue(alphaStr);





                objectsOnScene[i].program.Use();
                objectsOnScene[i].ChangeTransform();

                Gl.ActiveTexture(TextureUnit.Texture1);
                Gl.BindTexture(objectsOnScene[i].normal);
                Gl.ActiveTexture(TextureUnit.Texture0);
                Gl.BindTexture(objectsOnScene[i].diffuse);



                Gl.BindBufferToShaderAttribute(objectsOnScene[i].modelVertex, objectsOnScene[i].program, "vertexPosition");
                Gl.BindBufferToShaderAttribute(objectsOnScene[i].modelNormals, objectsOnScene[i].program, "vertexNormal");
                Gl.BindBufferToShaderAttribute(objectsOnScene[i].modelTangents, objectsOnScene[i].program, "vertexTangent");
                Gl.BindBufferToShaderAttribute(objectsOnScene[i].modelUV, objectsOnScene[i].program, "vertexUV");
                Gl.BindBuffer(objectsOnScene[i].modelElements);

                Gl.DrawElements(BeginMode.Triangles, objectsOnScene[i].modelElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
            this.SwapBuffers();

        }
    }
}
