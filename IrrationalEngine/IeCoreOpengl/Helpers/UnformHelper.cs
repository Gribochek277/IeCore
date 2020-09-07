using IeCoreInterfaces.Shaders;
using IeUtils;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace IeCoreOpengl.Helpers
{
    public class UniformHelper: IUniformHelper
    {
        private ILogger<UniformHelper> _logger;

        public UniformHelper(ILogger<UniformHelper> logger)
        {
            logger.AssertNotNull(nameof(logger));
            _logger = logger;
        }
        public bool TryAddUniform(int value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniformAddress(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform(float value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniformAddress(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform(double value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniformAddress(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform(Vector3 value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.Uniform3(shader.GetUniformAddress(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform(Vector3[] value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
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
                    GL.Uniform3(shader.GetUniformAddress(uniformName), 3 * value.Length, values);
                }
                catch (Exception e)
                {
                    //Possible memmory error.
                    _logger.LogError(e.Message);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform(Vector4 value, string uniformName, IShaderProgram shader)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.Uniform4(shader.GetUniformAddress(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniformTexture2D(int textureID, string uniformName, IShaderProgram shader, TextureUnit unit)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                GL.Uniform1(shader.GetUniformAddress(uniformName), (int)unit - 33984);//convertation of enum to int texture layer
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniformTextureCubemap(int textureID, string uniformName, IShaderProgram shader, TextureUnit unit)
        {
            if (shader.GetUniformAddress(uniformName) != -1)
            {
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
                GL.Uniform1(shader.GetUniformAddress(uniformName), (int)unit - 33984);//convertation of enum to int texture layer
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
