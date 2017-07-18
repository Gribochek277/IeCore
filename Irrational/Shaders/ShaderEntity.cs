using System;
using OpenTK.Graphics.OpenGL4;
namespace IrrationalSpace.Shaders
{
    public static class ShaderEntity
    {
		public static void LoadShader(String shader, ShaderType type, int programId, out int address)
		{
			address = GL.CreateShader(type);
			GL.ShaderSource(address, shader);
			GL.CompileShader(address);
			GL.AttachShader(programId, address);
			Console.WriteLine(GL.GetShaderInfoLog(address));
		}
    }
}
