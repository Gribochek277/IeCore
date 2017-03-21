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
        public enum ControlMode { FreeMouse,RotateModel, RotateCam };
        public static ControlMode controlMode = ControlMode.FreeMouse;
        public OpenTK.Vector2 lastMousePos = new OpenTK.Vector2();
        public OpenTK.Vector2 lastMousePos1 = new OpenTK.Vector2();

        public static bool autorotate = false;

        private static bool enableLight = true;

        private static List<Star> stars = new List<Star>();
        private static Random generator = new Random(Environment.TickCount);
        private static List<SceneObject> objectsOnScene = new List<SceneObject>();

        Camera cam = new Camera();

        public static float lightStr = 1f;
        public static bool fullscreen = false;
        public static bool alphaBlending  = false;
        public static float alphaStr = 2f;

        public WindowPreferences() : base(widght, height, GraphicsMode.Default, "Irrational engine",
            GameWindowFlags.Default, DisplayDevice.Default,
            // ask for an OpenGL 3.0 forward compatible context
            3, 0, GraphicsContextFlags.ForwardCompatible)
        { }

        protected override void OnLoad(EventArgs e)
        {
           

            Gl.Enable(EnableCap.DepthTest);
            Gl.Disable(EnableCap.Blend);
            SceneObject sceneObject = new SceneObject("resources/h.obj", new OpenGL.Vector3(-0.3f, -1.2f, 1), new OpenGL.Vector3(1, 1, 1) * 0.01f, new OpenGL.Vector3(1, 1, 1));
            sceneObject.SetMAterial("resources/h.jpg", "resources/hbump.jpg", true, new OpenGL.Vector3(0, 0, 1), lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);





            objectsOnScene.Add(sceneObject);
            objectsOnScene[0].program["projection_matrix"].SetValue(cam.GetViewMatrix() * OpenGL.Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f));
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
           
            if (controlMode == ControlMode.RotateModel)
            {
                OpenTK.Vector2 delta = lastMousePos - new OpenTK.Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                objectsOnScene[0].rotation.x -= delta.X * 0.005f;
                objectsOnScene[0].rotation.y -= delta.Y * 0.005f;
            }
            if (controlMode == ControlMode.RotateCam)
            {
                
                OpenTK.Vector2 mousedelta = lastMousePos - new OpenTK.Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                 cam.AddRotation(mousedelta.X, mousedelta.Y);
            }
            if(controlMode != ControlMode.FreeMouse)
            ResetCursor();

           
            objectsOnScene[0].program["projection_matrix"].SetValue(cam.GetViewMatrix() * OpenGL.Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f));
        }
        protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            Console.WriteLine((int)e.KeyChar);
            if (e.KeyChar == 92)
                Exit();

            if (e.KeyChar == 32)
            {
                controlMode = ((int)controlMode < 3) ? controlMode + 1 : ControlMode.FreeMouse;
                Console.WriteLine(controlMode.ToString());
            }

            switch ((int)e.KeyChar)
            {
                case 119:
                    {
                        cam.Move(0f, 1f, 0f);
                        Console.WriteLine("w");
                        break;
                    }
                   case 100:
                    {
                        cam.Move(1f, 0f, 0f);
                        Console.WriteLine("d");
                        break;
                    }
                    case 115:
                    {
                        cam.Move(0f, -1f, 0f);
                        Console.WriteLine("s");
                        break;
                    }
                    case 97:
                    {
                        cam.Move(-1f, 0f, 0f);
                        Console.WriteLine("a");
                        break;
                    }
            }
                
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

        void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Right / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new OpenTK.Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
    }
}
