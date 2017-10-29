using System;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Shaders
{
    public class UniformInfo
    {
		public String name = "";
		public int address = -1;
		public int size = 0;
		public ActiveUniformType type;
    }
}
