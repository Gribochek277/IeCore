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
       
       
       
        public static bool autorotate = false;
        private static List<Star> stars = new List<Star>();
        private static Random generator = new Random(Environment.TickCount);
        private static List<SceneObject> objectsOnScene = new List<SceneObject>();

      
        public static float lightStr = 1f;
        public static bool fullscreen = false;
        public static bool alphaBlending = false;
        public static float alphaStr =2f;
        

        static void Main(string[] args)
        {
			using (ApplicationWindow window = new ApplicationWindow())
			{
				window.Run(60);
			}
        }




 
        }}
