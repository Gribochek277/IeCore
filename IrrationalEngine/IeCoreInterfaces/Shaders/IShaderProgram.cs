//TODO: Consider this interface to re-factor/remove taking into account other realizations of renderer. 
using IeCoreEntites.Shaders;
using System;

namespace IeCoreInterfaces.Shaders
{
    /// <summary>
    /// Shader program couples together materials, textures and shaders.
    /// </summary>
    public interface IShaderProgram : IDisposable
    {
        /// <summary>
        /// Id of shader program.
        /// </summary>
        int ShaderProgramId { get; }
        /// <summary>
        /// Uses provided shader code as a string a loads it to program.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type">vertex, fragment etc.</param>
        /// <param name="shaderName"></param>
        void LoadShaderFromString(string code, string shaderName, ShaderType type);
        /// <summary>
        /// Get shader source from file and load it to program.
        /// </summary>
        /// <param name="filename"></param>
        /// /// <param name="shaderName"></param>
        /// <param name="type">vertex, fragment etc.</param>
        void LoadShaderFromFile(string filename, string shaderName, ShaderType type);
        /// <summary>
        /// Links compiled shader to program.
        /// </summary>
        void LinkShadersToProgram();
        /// <summary>
        /// Use current program for rendering.
        /// </summary>
        void UseProgram();
        /// <summary>
        /// Generate buffer object names;
        /// </summary>
        void GenBuffers();
        /// <summary>
        /// Enables generic vertex attribute arrays.
        /// </summary>
        void EnableVertexAttribArrays();
        /// <summary>
        /// Disables generic vertex attribute arrays.
        /// </summary>
        void DisableVertexAttribArrays();
        /// <summary>
        /// Gets attribute by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetAttributeAddress(string name);
        /// <summary>
        /// Gets uniform by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetUniformAddress(string name);
        /// <summary>
        /// Gets buffer by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        uint GetBuffer(string name);
    }
}
