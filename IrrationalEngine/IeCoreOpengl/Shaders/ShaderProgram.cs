using IeCoreEntities.Shaders;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Shaders;
using IeUtils;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using ShaderType = IeCoreEntities.Shaders.ShaderType;

namespace IeCoreOpengl.Shaders
{
	public class ShaderProgram : IShaderProgram
	{
		private readonly List<Shader> _storedShaders = new List<Shader>();
		private readonly IAssetManager _assetManager;
		private readonly ILogger<ShaderProgram> _logger;


		private Dictionary<string, AttributeInfo> _attributeInfos;


		private readonly Dictionary<string, uint> _buffers = new Dictionary<string, uint>();


		private Dictionary<string, UniformInfo> _uniforms;

		public int ShaderProgramId { get; private set; } = -1;

		private bool _disposedValue;

		public ShaderProgram(IAssetManager assetManager, ILogger<ShaderProgram> logger)
		{
			assetManager.AssertNotNull(nameof(assetManager));
			logger.AssertNotNull(nameof(logger));
			_assetManager = assetManager;
			_logger = logger;
		}


		public void DisableVertexAttribArrays()
		{
			throw new NotImplementedException();
		}

		public void EnableVertexAttribArrays()
		{
			foreach (var attributeInfo in _attributeInfos)
			{
				GL.EnableVertexAttribArray(attributeInfo.Value.Address);
			}
		}

		public void GenBuffers()
		{
			for (var i = 0; i < _attributeInfos.Count; i++)
			{
				if (_buffers.ContainsKey(_attributeInfos.Values.ElementAt(i).Name)) continue;

				GL.GenBuffers(1, out uint buffer);
				_buffers.Add(_attributeInfos.Values.ElementAt(i).Name, buffer);
			}

			for (var i = 0; i < _uniforms.Count; i++)
			{
				if (_buffers.ContainsKey(_uniforms.Values.ElementAt(i).Name)) continue;

				GL.GenBuffers(1, out uint buffer);
				_buffers.Add(_uniforms.Values.ElementAt(i).Name, buffer);
			}
		}

		public int GetAttributeAddress(string name)
		{
			try { 
				return _attributeInfos[name].Address;
			}
			catch 
			{ 
				return -1; 
			}
		}

		public uint GetBuffer(string name)
		{
			return _buffers.ContainsKey(name) ? _buffers[name] : 0;
		}

		public int GetUniformAddress(string name)
		{
			_uniforms.TryGetValue(name, out UniformInfo result);
			return result?.Address ?? -1;
		}

		public void LinkShadersToProgram()
		{
			foreach (Shader storedShader in _storedShaders)
			{
				GL.AttachShader(ShaderProgramId, storedShader.Id);
			}

			GL.LinkProgram(ShaderProgramId);

			_attributeInfos = GetAttributesInformation(ShaderProgramId);
			_uniforms = GetUniformsInformation(ShaderProgramId);

			//After linking program we can detach shaders.
			foreach (Shader storedShader in _storedShaders)
			{
				GL.DetachShader(ShaderProgramId, storedShader.Id);
			}
		}

		private static Dictionary<string, AttributeInfo> GetAttributesInformation(int shaderProgramId)
		{
			GL.GetProgram(shaderProgramId, GetProgramParameterName.ActiveAttributes, out int attributeCount);
			var attributes = new Dictionary<string, AttributeInfo>();
			for (var i = 0; i < attributeCount; i++)
			{
				var info = new AttributeInfo();
				GL.GetActiveAttrib(shaderProgramId, i, 256, out int length, out info.Size, out ActiveAttribType type, out string name);

				info.Name = name;
				info.Address = GL.GetAttribLocation(shaderProgramId, info.Name);
				info.Type = (int)type;
				attributes.Add(name, info);
			}

			return attributes;
		}

		private static Dictionary<string, UniformInfo> GetUniformsInformation(int shaderProgramId)
		{
			GL.GetProgram(shaderProgramId, GetProgramParameterName.ActiveUniforms, out int uniformCount);
			var uniforms = new Dictionary<string, UniformInfo>();
			for (var i = 0; i < uniformCount; i++)
			{
				var info = new UniformInfo();
				GL.GetActiveUniform(shaderProgramId, i, 256, out int length, out info.Size, out ActiveUniformType type, out string name);

				info.Name = name;
				info.Address = GL.GetUniformLocation(shaderProgramId, info.Name);
				info.Type = (int)type;
				uniforms.Add(name, info);
			}

			return uniforms;
		}

		public void UseProgram()
		{
			GL.UseProgram(ShaderProgramId);
		}

		public void LoadShaderFromFile(string filename, string shaderName, ShaderType type)
		{
			throw new NotImplementedException();
		}

		public void LoadShaderFromString(string code, string shaderName, ShaderType type)
		{
			ShaderProgramId = GL.CreateProgram();
			var registeredShader = _assetManager.Retrieve<Shader>(shaderName);
			if (registeredShader == null)
			{
				var shader = new Shader(shaderName, string.Concat("InMemory shader ", Guid.NewGuid().ToString()), code, type);

				_assetManager.Register(shader);
				_storedShaders.Add(shader);

				var oglEnum = Enum.Parse<OpenTK.Graphics.OpenGL.ShaderType>(shader.ShaderType.ToString());

				shader.Id = GL.CreateShader(oglEnum);
				GL.ShaderSource(shader.Id, shader.ShaderCode);
				GL.CompileShader(shader.Id);

				string shaderInfo = GL.GetShaderInfoLog(shader.Id);
				if (!string.IsNullOrEmpty(shaderInfo))
					_logger.LogError(shaderInfo); //Log errors.
				else
					_logger.LogInformation($"{shaderName} compiled correctly");
			}
			else
			{
				_storedShaders.Add(registeredShader);
			}
		}


		protected virtual void Dispose(bool disposing)
		{
			if (_disposedValue) return;
			GL.DeleteProgram(ShaderProgramId);

			_disposedValue = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
