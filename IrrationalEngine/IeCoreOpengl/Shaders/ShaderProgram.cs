using IeCoreEntites.Shaders;
using IeCoreInterfaces.Shaders;
using OpenToolkit.Graphics.OpenGL;
using System;
using IeUtils;
using IeCoreInterfaces.Assets;
using System.Collections.Generic;
using System.Linq;

namespace IeCoreOpengl.Shaders
{
    public class ShaderProgram : IShaderProgram
    {
        private List<Shader> _storedShaders = new List<Shader>();
        private IAssetManager _assetmanager;

        /// <summary>
        /// Dictionary of attribute info contained in shader.
        /// </summary>
        public Dictionary<string, AttributeInfo> AttributeInfos { get; private set; }

        /// <summary>
        /// Dictionary of uniforms contained in shader.
        /// </summary>
        public Dictionary<string, UniformInfo> Uniforms { get; private set; }

        public int ShaderProgramId { get; private set; } = -1;

        private bool disposedValue = false;

        public ShaderProgram(IAssetManager assetmanager)
        {
            assetmanager.AssertNotNull(nameof(assetmanager));
            _assetmanager = assetmanager;
            ShaderProgramId = GL.CreateProgram();
        }


        public void DisableVertexAttribArrays()
        {
            throw new NotImplementedException();
        }

        public void EnableVertexAttribArrays()
        {
            for (int i = 0; i < AttributeInfos.Count; i++)
            {
                GL.EnableVertexAttribArray(AttributeInfos.Values.ElementAt(i).Address);
            }
        }

        public void GenBuffers()
        {
            throw new NotImplementedException();
        }

        public int GetAttributeAddress(string name)
        {
            return AttributeInfos[name].Address;
        }

        public uint GetBuffer(string name)
        {
            throw new NotImplementedException();
        }

        public int GetUniformAddress(string name)
        {
            return Uniforms[name].Address;
        }

        public void LinkShadersToProgram()
        {
            foreach (Shader storedShader in _storedShaders)
            {
                GL.AttachShader(ShaderProgramId, storedShader.Id);
            }

            GL.LinkProgram(ShaderProgramId);

            AttributeInfos = GetAttributesInformation(ShaderProgramId);
            Uniforms = GetUniformsInformation(ShaderProgramId);

            //After linking program we can detach shaders.
            foreach (Shader storedShader in _storedShaders)
            {
                GL.DetachShader(ShaderProgramId, storedShader.Id);
            }
        }

        private Dictionary<string, AttributeInfo> GetAttributesInformation(int shaderProgramId)
        {
            GL.GetProgram(shaderProgramId, GetProgramParameterName.ActiveAttributes, out int AttributeCount);
            Dictionary<string, AttributeInfo> attributes = new Dictionary<string, AttributeInfo>();
            for (int i = 0; i < AttributeCount; i++)
            {
                AttributeInfo info = new AttributeInfo();
                GL.GetActiveAttrib(shaderProgramId, i, 256, out int length, out info.Size, out ActiveAttribType type, out string name);

                info.Name = name.ToString();
                info.Address = GL.GetAttribLocation(shaderProgramId, info.Name);
                info.Type = (int)type;
                attributes.Add(name.ToString(), info);
            }

            return attributes;
        }

        private Dictionary<string, UniformInfo> GetUniformsInformation(int shaderProgramId)
        {
            GL.GetProgram(shaderProgramId, GetProgramParameterName.ActiveUniforms, out int UniformCount);
            Dictionary<string, UniformInfo> uniforms = new Dictionary<string, UniformInfo>();
            for (int i = 0; i < UniformCount; i++)
            {
                UniformInfo info = new UniformInfo();
                GL.GetActiveUniform(shaderProgramId, i, 256, out int length, out info.Size, out ActiveUniformType type, out string name);

                info.Name = name.ToString();
                info.Address = GL.GetUniformLocation(shaderProgramId, info.Name);
                info.Type = (int)type;
                uniforms.Add(name.ToString(), info);
            }

            return uniforms;
        }

        public void UseProgram()
        {
            GL.UseProgram(ShaderProgramId);
        }

        public void LoadShaderFromFile(string filename, string shaderName, IeCoreEntites.Shaders.ShaderType type)
        {
            throw new NotImplementedException();
        }

        public void LoadShaderFromString(string code, string shaderName, IeCoreEntites.Shaders.ShaderType type)
        {
            Shader registeredShader = _assetmanager.Retrieve<Shader>(shaderName);
            if (registeredShader == null)
            {
                Shader shader = new Shader(shaderName, string.Concat("InMemmory shader ", Guid.NewGuid()), code, type);

                _assetmanager.Register(shader);
                _storedShaders.Add(shader);

                OpenToolkit.Graphics.OpenGL.ShaderType OGLEnum = Enum.Parse<OpenToolkit.Graphics.OpenGL.ShaderType>(shader.ShaderType.ToString());

                shader.Id = GL.CreateShader(OGLEnum);
                GL.ShaderSource(shader.Id, shader.ShaderCode);
                GL.CompileShader(shader.Id);

                string shaderInfo = GL.GetShaderInfoLog(shader.Id);
                if (!string.IsNullOrEmpty(shaderInfo))
                    Console.WriteLine(shaderInfo); //Log errors.
                else
                    Console.WriteLine($"{shaderName} compiled correctly");
            }
            else
            {
                _storedShaders.Add(registeredShader);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(ShaderProgramId);

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
