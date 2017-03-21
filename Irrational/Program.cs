using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;


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




            WindowPreferences window = new WindowPreferences();
            



            // SceneObject sceneObject2 = new SceneObject("model.txt",new Vector3(1, -1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1));
            //sceneObject2.SetMAterial("african_head_diffuse.jpg", true, new Vector3(0, 0, 1), lightStr, alphaStr, SceneObject.VertextShader, SceneObject.FragmentShader);

            //objectsOnScene.Add(sceneObject2);
            watch = System.Diagnostics.Stopwatch.StartNew();
            window.Run(60);
        }




 
        }}
