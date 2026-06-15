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

		public static unsafe bool TryAddUniform(GL gl, Matrix4x4[] value, string uniformName, IShaderProgram shader)
		{
			if (shader.GetUniformAddress(uniformName) != -1)
			{
				const int MaxBones = 100;
				if (value.Length > MaxBones)
				{
					throw new ArgumentException($"Bone matrix count {value.Length} exceeds shader MAX_BONES {MaxBones}");
				}

				float[] floats = new float[value.Length * 16];
				for (int i = 0; i < value.Length; i++)
				{
					Matrix4x4 m = value[i];
					int offset = i * 16;
					floats[offset + 0] = m.M11; floats[offset + 1] = m.M12; floats[offset + 2] = m.M13; floats[offset + 3] = m.M14;
					floats[offset + 4] = m.M21; floats[offset + 5] = m.M22; floats[offset + 6] = m.M23; floats[offset + 7] = m.M24;
					floats[offset + 8] = m.M31; floats[offset + 9] = m.M32; floats[offset + 10] = m.M33; floats[offset + 11] = m.M34;
					floats[offset + 12] = m.M41; floats[offset + 13] = m.M42; floats[offset + 14] = m.M43; floats[offset + 15] = m.M44;
				}
				fixed (float* ptr = floats)
				{
					gl.UniformMatrix4(shader.GetUniformAddress(uniformName), (uint)value.Length, false, ptr);
				}
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
