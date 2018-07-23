using Irrational.Core.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Renderer.OpenGL
{
    public interface IUniformHelper
    {
        bool TryAddUniform1(float value, string uniformName, ShaderProg shader);
        bool TryAddUniform1(double value, string uniformName, ShaderProg shader);
        bool TryAddUniform1(int value, string uniformName, ShaderProg shader);
        bool TryAddUniform1(Vector3 value, string uniformName, ShaderProg shader);
        bool TryAddUniformTexture2D(int textureID, string uniformName, ShaderProg shader, TextureUnit unit);
    }

}
