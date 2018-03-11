using Irrational.Core.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Irrational.Core.Renderer.OpenGL
{
    class UniformHelper : IUniformHelper
    {
        public bool TryAddUniform1(int value, string uniformName, ShaderProg shader)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniform(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform1(float value, string uniformName, ShaderProg shader)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniform(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform1(double value, string uniformName, ShaderProg shader)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.Uniform1(shader.GetUniform(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform1(Vector3 value, string uniformName, ShaderProg shader)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.Uniform3(shader.GetUniform(uniformName), value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniform1(Vector3[] value, string uniformName, ShaderProg shader)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                float[] values = new float[value.Length * 3];
                for (int i = 0; i < value.Length; ++i)
                {
                    values[i * 3] = value[i].X;
                    values[i * 3+1] = value[i].Y;
                    values[i * 3+2] = value[i].Z;
                }
                try
                {
                    GL.Uniform3(shader.GetUniform(uniformName), 3 * value.Length, values);
                }
                catch (Exception e)
                {
                    //Possible memmory error.
                    Console.WriteLine(e.Message);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniformTexture2D(int textureID, string uniformName, ShaderProg shader, TextureUnit unit)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                GL.Uniform1(shader.GetUniform(uniformName), (int)unit-33984);//convertation of enum to int texture layer
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddUniformTextureCubemap(int textureID, string uniformName, ShaderProg shader, TextureUnit unit)
        {
            if (shader.GetUniform(uniformName) != -1)
            {
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
                GL.Uniform1(shader.GetUniform(uniformName), (int)unit - 33984);//convertation of enum to int texture layer
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
