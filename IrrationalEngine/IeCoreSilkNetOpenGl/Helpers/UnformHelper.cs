using System.Numerics;
using IeCoreInterfaces.Shaders;
using Silk.NET.OpenGL;

namespace IeCoreSilkNetOpenGl.Helpers
{
	public static class UniformHelper
	{
		public static bool TryAddUniform(GL gl, int value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.Uniform1(shader.GetUniformAddress(uniformName), value);
				return true;
			}

			return false;
		}

		public static bool TryAddUniform(GL gl,float value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.Uniform1(shader.GetUniformAddress(uniformName), value);
				return true;
			}

			return false;
		}

		public static bool TryAddUniform(GL gl,double value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.Uniform1(shader.GetUniformAddress(uniformName), value);
				return true;
			}

			return false;
		}

		public static bool TryAddUniform(GL gl,Vector3 value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.Uniform3(shader.GetUniformAddress(uniformName), value);
				return true;
			}

			return false;
		}

		public static bool TryAddUniform(GL gl, Vector3[] value, string uniformName, IShaderProgram shader)
		{
			throw new NotImplementedException();
			/*if (shader.GetUniformAddress(uniformName) != -1)
			{
				float[] values = new float[value.Length * 3];
				for (int i = 0; i < value.Length; ++i)
				{
					values[i * 3] = value[i].X;
					values[i * 3 + 1] = value[i].Y;
					values[i * 3 + 2] = value[i].Z;
				}
				try
				{
					gl.Uniform3(shader.GetUniformAddress(uniformName), 3 * value.Length, values);
				}
				catch (Exception e)
				{
					//Possible memory error.
					return false;
				}

				return true;
			}

			return false;*/
		}

		public static bool TryAddUniform(GL gl, Vector4 value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.Uniform4(shader.GetUniformAddress(uniformName), value);
				return true;
			}

			return false;
		}

		public static bool TryAddUniformTexture2D(GL gl, int textureId, string uniformName, IShaderProgram shader, TextureUnit unit)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.ActiveTexture(unit);
				gl.BindTexture(TextureTarget.Texture2D, (uint)textureId);
				gl.Uniform1(shader.GetUniformAddress(uniformName), (int)unit - 33984);//convertation of enum to int texture layer
				return true;
			}

			return false;
		}

		public static bool TryAddUniformTextureCubemap(GL gl, int textureId, string uniformName, IShaderProgram shader, TextureUnit unit)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				gl.ActiveTexture(unit);
				gl.BindTexture(TextureTarget.TextureCubeMap, (uint)textureId);
				gl.Uniform1(shader.GetUniformAddress(uniformName), (int)unit - 33984);//convertation of enum to int texture layer
				return true;
			}

			return false;
		}
	}
}
