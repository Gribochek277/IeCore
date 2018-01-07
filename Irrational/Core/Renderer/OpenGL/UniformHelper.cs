using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irrational.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
    }
}
