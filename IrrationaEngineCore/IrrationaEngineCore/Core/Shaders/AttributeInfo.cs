using System;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Shaders
{
    public class AttributeInfo
    {
		public String name = "";
		public int address = -1;
		public int size = 0;
		public ActiveAttribType type;
    }
}
