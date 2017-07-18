using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IrrationalSpace
{
    class Program
    {
		static void Main(string[] args)
        {
			using (ApplicationWindow window = new ApplicationWindow())
			{
				//Framerate as parameter
				window.VSync = OpenTK.VSyncMode.Adaptive;
				window.Run(60,60);
			}
        }
	}
}
