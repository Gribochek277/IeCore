using IeCoreInterfaces.Shaders;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;

namespace IeCoreOpengl.Helpers
{
    public interface IUniformHelper
    {
        bool TryAddUniform(int value, string uniformName, IShaderProgram shader);

        bool TryAddUniform(float value, string uniformName, IShaderProgram shader);

        bool TryAddUniform(double value, string uniformName, IShaderProgram shader);

        bool TryAddUniform(Vector3 value, string uniformName, IShaderProgram shader);

        bool TryAddUniform(Vector3[] value, string uniformName, IShaderProgram shader);

        bool TryAddUniform(Vector4 value, string uniformName, IShaderProgram shader);

        bool TryAddUniformTexture2D(int textureID, string uniformName, IShaderProgram shader, TextureUnit unit);

        bool TryAddUniformTextureCubemap(int textureID, string uniformName, IShaderProgram shader, TextureUnit unit);
    }
}
