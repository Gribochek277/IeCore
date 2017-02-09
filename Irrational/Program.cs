using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using OpenGL;
using Gtk;

namespace IrrationalSpace
{
    class Program
    {
       
       
        private static System.Diagnostics.Stopwatch watch;
        private static float xangle, yangle;
        public static bool autorotate = false;

        private static  bool enableLight = true;
       
        private static List<Star> stars = new List<Star>();
        private static Random generator = new Random(Environment.TickCount);
        private static List<SceneObject> objectsOnScene = new List<SceneObject>();

        private static int currentObject = 0;
       
        public static float lightStr = 1f;
        public static bool fullscreen = false;
        public static bool alphaBlending = false;
        public static float alphaStr =2f;
        

        static void Main(string[] args)
        {
			Application.Init();

			Window win = new Window("Properties");

			win.Resize(200, 400);

			Label label = new Label();

			label.Text = "hihihi";

			win.Add(label);

			win.ShowAll();




			WindowPreferences.InitWindow(OnRenderFrame, OnDisplay, OnKeyboardDown, OnKeyboardUp, OnClose, OnReshape,OnMouse,OnMove,800,600);

            SceneObject sceneObject = new SceneObject("resources/h.obj", new Vector3(-0.3f,-1.2f,1),new Vector3(1,1,1)*0.01f,new Vector3(1,1,1));
            sceneObject.SetMAterial("resources/h.jpg", "resources/hbump.jpg", true, new Vector3(0, 0, 1),lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);



            objectsOnScene.Add(sceneObject);
           

            // SceneObject sceneObject2 = new SceneObject("model.txt",new Vector3(1, -1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1));
            //sceneObject2.SetMAterial("african_head_diffuse.jpg", true, new Vector3(0, 0, 1), lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);

            //objectsOnScene.Add(sceneObject2);
            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();
			Application.Run();
        }

        private static void OnDisplay()
        {
            
        }
        private static void OnReshape(int width, int height)
        {
            WindowPreferences.widght = width;
            WindowPreferences.height = height;
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                Gl.Viewport(0, 0, WindowPreferences.widght, WindowPreferences.height);
                objectsOnScene[i].program.Use();
                objectsOnScene[i].program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)WindowPreferences.widght / WindowPreferences.height, 0.1f, 1000f));
            }
        }
       
        private static void OnClose()
        {
            for (int i = 0; i < objectsOnScene.Count; i++)
            {
                objectsOnScene[i].program.DisposeChildren = true;
                objectsOnScene[i].program.Dispose();
            }
        }
      
        private static void OnRenderFrame()
        {
            watch.Stop();
            float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

           
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



            Glut.glutSwapBuffers();
        }
        
       










          private static void OnKeyboardDown(byte key, int x, int y)
        {

            if (key == 9)
            {
                currentObject++;
                if (currentObject == objectsOnScene.Count)
                {
                    currentObject = 0;
                }

            }


            if (key == 27) Glut.glutLeaveMainLoop();

            if (key == 't')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.x += 10 * deltaTime;
            }
            if (key == 'g')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.x -= 10 * deltaTime;
            }
            if (key == 'f')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.y += 10 * deltaTime;
            }
            if (key == 'h')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.y -= 10 * deltaTime;
            }
            if (key == 'r')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.z += 10 * deltaTime;
            }
            if (key == 'y')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].position.z -= 10 * deltaTime;
            }
            if (key == 'q')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.z += 10 * deltaTime;
            }
            if (key == 'e')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.z -= 10 * deltaTime;
            }
            if (key == 'w')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.x += 10 * deltaTime;
            }
            if (key == 's')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.x -= 10 * deltaTime;
            }
            if (key == 'd')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.y += 10f * deltaTime;
            }
            if (key == 'a')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                objectsOnScene[currentObject].rotation.y -= 10f * deltaTime;
            }

            if (key == '=')
            {
                lightStr += 0.2f;
            }
            if (key == '-')
            {
                lightStr -= 0.2f;
            }
            if (key == '9')
            {
                alphaStr += 0.1f;
            }
            if (key == '0')
            {
                alphaStr -= 0.1f;
            }

        }

        private static void OnKeyboardUp(byte key, int x, int y)
        {
           // Console.WriteLine(key);
            if (key == 108)
            {
                enableLight = !enableLight;
            }
            if (key == 'b')
            {
                alphaBlending = !alphaBlending;
                if (alphaBlending)
                {
                    Gl.Disable(EnableCap.DepthTest);
                    Gl.Enable(EnableCap.Blend);
                }
                else
                {
                    Gl.Enable(EnableCap.DepthTest);
                    Gl.Disable(EnableCap.Blend);
                }
            }
            if (key == ' ')
            {
                autorotate = !autorotate;
            }




            if (key == 'x')
            {
                fullscreen = !fullscreen;
                if (fullscreen) Glut.glutFullScreen();
                else
                {
                    Glut.glutPositionWindow(0, 0);
                    Glut.glutReshapeWindow(800, 600);

                }
            }
        }

		private static bool mouseDown = false, wheelDown = false;
		private static int downX, downY;

		private static void OnMouse(int button, int state, int x, int y)
		{
			// this method gets called whenever a new mouse button event happens
			if (button == Glut.GLUT_RIGHT_BUTTON) mouseDown = (state == Glut.GLUT_DOWN);

			// if the mouse has just been clicked then we hide the cursor and store the position
			if (mouseDown)
			{
				Glut.glutSetCursor(Glut.GLUT_CURSOR_NONE);
				downX = x;
				downY = y;
			}
			else // unhide the cursor if the mouse has just been released
				Glut.glutSetCursor(Glut.GLUT_CURSOR_LEFT_ARROW);
		}


		private static void OnMove(int x, int y)
		{
			// if the mouse move event is caused by glutWarpPointer then do nothing
			if (x == downX && y == downY) return;

			// update the rotation of our cube if the mouse is down
			if (mouseDown)
			{
				 


				objectsOnScene[currentObject].rotation.x += (x - downX) * 0.005f;

				objectsOnScene[currentObject].rotation.y -=  (y - downY) * 0.005f;


				Glut.glutWarpPointer(downX, downY);
			}
		}

	

	}
}
