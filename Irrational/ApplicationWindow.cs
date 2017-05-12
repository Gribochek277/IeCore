using System;
using OpenGL;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Input;

namespace IrrationalSpace
{
    public class ApplicationWindow : GameWindow
    {
        public static int widght = 800, height = 600;
        public enum ControlMode { FreeMouse,RotateModel, RotateCam };
        public static ControlMode controlMode = ControlMode.FreeMouse;
        public OpenTK.Vector2 lastMousePos = new OpenTK.Vector2();
       
		//TODO: this should be in scene entity. thisnk how to inject to it;
		private static List<ISceneObject> objectsOnScene = new List<ISceneObject>();
		private Scene curretnScene;
        
        public static bool fullscreen = false;
        public static bool alphaBlending  = false;
        public static float alphaStr = 2f;

		public ApplicationWindow() : base(widght, height, GraphicsMode.Default, "Irrational engine",
            GameWindowFlags.Default, DisplayDevice.Default,
            // ask for an OpenGL 3.0 forward compatible context
            3, 0, GraphicsContextFlags.ForwardCompatible)
        { }

        protected override void OnLoad(EventArgs e)
        {
			curretnScene = new Scene() { LightDirection = new OpenGL.Vector3(0, 0, 1), MainCamera = new Camera(), EnableLight = true, LightStr = 1f};
            Gl.Enable(EnableCap.DepthTest);
            Gl.Disable(EnableCap.Blend);
            SceneObject sceneObject = new SceneObject("resources/h.obj", new OpenGL.Vector3(-0.3f, -1.2f, 1), new OpenGL.Vector3(1, 1, 1) * .01f, new OpenGL.Vector3(1, 1, 1));

			sceneObject.mat = new Material() 
			{ 
				diffuse=new Texture("resources/h.jpg"),
				normal =  new Texture("resources/hbump.jpg"), 
				shader = new ShaderProgram(VertexShaders.VertexShaderDefault,FragmentShaders.FragmentShaderDefault)
			};
			sceneObject.scene = curretnScene;
			sceneObject.SetMAterial();

            objectsOnScene.Add(sceneObject);
			Gl.BlendFunc(BlendingFactorSrc.OneMinusConstantAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }
        
        protected override void OnResize(EventArgs e)
        {
            ApplicationWindow.widght = this.Width;
            ApplicationWindow.height = this.Height;
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                Gl.Viewport(0, 0, ApplicationWindow.widght, ApplicationWindow.height);
				objectsOnScene[i].mat.shader.Use();
				objectsOnScene[i].mat.shader["projection_matrix"].SetValue(OpenGL.Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)ApplicationWindow.widght / ApplicationWindow.height, 0.1f, 1000f));
            }
        }

        private void OnClose()
        {
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
				objectsOnScene[i].mat.shader.DisposeChildren = true;
				objectsOnScene[i].mat.shader.Dispose();
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // this is called every frame, put game logic here
           
            if (controlMode == ControlMode.RotateModel)
            {
                OpenTK.Vector2 delta = lastMousePos - new OpenTK.Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
				OpenGL.Vector3 rotate = objectsOnScene[0].rotation;
				rotate.x -= delta.X * 0.005f;
				rotate.y -= delta.Y * 0.005f;
				objectsOnScene[0].rotation = rotate;
            }
            if (controlMode == ControlMode.RotateCam)
            {    
                OpenTK.Vector2 mousedelta = lastMousePos - new OpenTK.Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
				curretnScene.MainCamera.AddRotation(mousedelta.X, mousedelta.Y);
			}
			if(controlMode != ControlMode.FreeMouse)
            ResetCursor();

           
			objectsOnScene[0].mat.shader["projection_matrix"].SetValue(curretnScene.MainCamera.GetViewMatrix() * OpenGL.Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f));
			objectsOnScene[0].ChangeTransform();
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
						curretnScene.MainCamera.Move(0f, 1f, 0f);
                        Console.WriteLine("w");
                        break;
                    }
                   case 100:
                    {
                        curretnScene.MainCamera.Move(1f, 0f, 0f);
                        Console.WriteLine("d");
                        break;
                    }
                    case 115:
                    {
                        curretnScene.MainCamera.Move(0f, -1f, 0f);
                        Console.WriteLine("s");
                        break;
                    }
                    case 97:
                    {
                        curretnScene.MainCamera.Move(-1f, 0f, 0f);
                        Console.WriteLine("a");
                        break;
                    }
					 case 108:
                    {
						curretnScene.EnableLight = !curretnScene.EnableLight;
                        Console.WriteLine("l");
                        break;
                    }
            }
                
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
				objectsOnScene[i].mat.shader["enable_lighting"].SetValue(curretnScene.EnableLight);
				objectsOnScene[i].mat.shader["light_strenght"].SetValue(curretnScene.LightStr);
                objectsOnScene[i].mat.shader["alpha_str"].SetValue(alphaStr);

                objectsOnScene[i].mat.shader.Use();
               
                Gl.ActiveTexture(TextureUnit.Texture1);
				Gl.BindTexture(objectsOnScene[i].mat.normal);
                Gl.ActiveTexture(TextureUnit.Texture0);
				Gl.BindTexture(objectsOnScene[i].mat.diffuse);

				Gl.BindBufferToShaderAttribute(objectsOnScene[i].model.modelVertex, objectsOnScene[i].mat.shader, "vertexPosition");
				Gl.BindBufferToShaderAttribute(objectsOnScene[i].model.modelNormals, objectsOnScene[i].mat.shader, "vertexNormal");
				Gl.BindBufferToShaderAttribute(objectsOnScene[i].model.modelTangents, objectsOnScene[i].mat.shader, "vertexTangent");
				Gl.BindBufferToShaderAttribute(objectsOnScene[i].model.modelUV, objectsOnScene[i].mat.shader, "vertexUV");
				Gl.BindBuffer(objectsOnScene[i].model.modelElements);

				Gl.DrawElements(BeginMode.Triangles, objectsOnScene[i].model.modelElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
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
