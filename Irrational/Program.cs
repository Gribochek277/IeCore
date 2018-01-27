using Irrational.Core.Windows;
using Irrational.Utils;

namespace Irrational
{
    class Program
    {
		static void Main(string[] args)
        {
            Gltf2ModelLoader loader = new Gltf2ModelLoader();
            loader.LoadFromFile("Resources/Gltf/Damagedhelmet/glTF/DamagedHelmet.gltf");
            OpenTKWindow window = new OpenTKWindow();
            window.Run();
        }
	}
}
