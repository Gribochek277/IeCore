using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using OpenGL;

namespace OpenGLTUT
{
    class Program
    {
        

         public static ShaderProgram program;
       
        // private static VBO<Vector3> /*pyramidColors,*/ cuberColors;
       
        private static System.Diagnostics.Stopwatch watch;
        private static float xangle, yangle;
        public static bool autorotate = false;
        private static Texture texture;
        private static  bool enableLight = true;
        private static BeginMode drawingMode;
        private static List<Star> stars = new List<Star>();
        private static Random generator = new Random(Environment.TickCount);
        private static List<SceneObject> objectsOnScene = new List<SceneObject>();
       
        public static float lightStr = 1f;
        public static bool fullscreen = false;
        public static bool alphaBlending = false;
        public static float alphaStr =2f;
        

        static void Main(string[] args)
        {
            WindowPreferences.InitWindow(OnRenderFrame, OnDisplay, OnKeyboardDown, OnKeyboardUp, OnClose, OnReshape,1280,720);

            SceneObject sceneObject = new SceneObject("star-wars-arc.obj");
            sceneObject.SetMAterial("Arc170_blinn1.png",true, new Vector3(0, 0, 1),lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);
            program = sceneObject.program;
            texture = sceneObject.texture;
            drawingMode = BeginMode.Triangles;

            objectsOnScene.Add(sceneObject);
           

            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();
        }

        private static void OnDisplay()
        {
            
        }
        private static void OnReshape(int width, int height)
        {
            WindowPreferences.widght = width;
            WindowPreferences.height = height;
            Gl.Viewport(0, 0, WindowPreferences.widght, WindowPreferences.height);
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)WindowPreferences.widght / WindowPreferences.height, 0.1f, 1000f));
        }
      
       
        private static void OnClose()
        {
            program.DisposeChildren = true;
            program.Dispose();
        }
      
        private static void OnRenderFrame()
        {
            watch.Stop();
            float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

                if (autorotate)
                {
                    xangle += deltaTime;
                    yangle += deltaTime;
                }

                program["enable_lighting"].SetValue(enableLight);
                program["light_strenght"].SetValue(lightStr);
                program["alpha_str"].SetValue(alphaStr);
                

               
                Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                program.Use();

            program["model_matrix"].SetValue(Matrix4.CreateRotationX(xangle) * Matrix4.CreateRotationY(yangle / 2) * Matrix4.CreateScaling(new Vector3(1, 1, 1)*0.1f) /** Matrix4.CreateTranslation(new Vector3(1.5f, 0, 0))*/);
                Gl.BindBufferToShaderAttribute(objectsOnScene[0].modelVertex, program, "vertexPosition");
                Gl.BindBufferToShaderAttribute(objectsOnScene[0].modelNormals, program, "vertexNormal");
                Gl.BindBufferToShaderAttribute(objectsOnScene[0].modelUV, program, "vertexUV");
                Gl.BindBuffer(objectsOnScene[0].modelElements);

                Gl.DrawElements(drawingMode, objectsOnScene[0].modelElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
           



            Glut.glutSwapBuffers();
        }
        
       










          private static void OnKeyboardDown(byte key, int x, int y)
        {




            if (key == 27) Glut.glutLeaveMainLoop();
            if (key == 'w')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                xangle += 10 * deltaTime;
            }
            if (key == 's')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                xangle -= 10 * deltaTime;
            }
            if (key == 'd')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                yangle += 10f * deltaTime;
            }
            if (key == 'a')
            {

                float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
                yangle -= 10f * deltaTime;
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
            Console.WriteLine(key);
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




            if (key == 'f')
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
    }
}
