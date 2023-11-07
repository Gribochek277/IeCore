using IeCoreEntities.Shaders;
using IeCoreInterfaces.Assets;
using IeCoreInterfaces.Shaders;
using IeCoreSilkNetOpenGl.EngineWindow;
using IeUtils;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenGL;
using Shader = IeCoreEntities.Shaders.Shader;
using ShaderType = IeCoreEntities.Shaders.ShaderType;

namespace IeCoreSilkNetOpenGl.Shaders;

public class ShaderProgram: IShaderProgram
{
	private readonly List<Shader> _storedShaders = new List<Shader>();
	private readonly IAssetManager _assetManager;
	private readonly ILogger<ShaderProgram> _logger;


	private Dictionary<string, AttributeInfo> _attributeInfos;


	private readonly Dictionary<string, uint> _buffers = new Dictionary<string, uint>();


	private Dictionary<string, UniformInfo> _uniforms;

	public int ShaderProgramId { get; private set; } = -1;
	
	private static GL _gl;
	private bool _disposedValue;
	
	public ShaderProgram(IAssetManager assetManager, ILogger<ShaderProgram> logger)
	{
		assetManager.AssertNotNull(nameof(assetManager));
		logger.AssertNotNull(nameof(logger));
		_assetManager = assetManager;
		_logger = logger;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposedValue) return;
		//GL.DeleteProgram(ShaderProgramId);

		_disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public void LoadShaderFromString(string code, string shaderName, ShaderType type)
	{
		_gl = SilkNetOpenGlWindow.GetWindowContext;
		ShaderProgramId = (int)_gl.CreateProgram();
		Shader? registeredShader = _assetManager.Retrieve<Shader>(shaderName);
		if (registeredShader == null)
		{
			Shader shader = new Shader(shaderName, string.Concat("InMemory shader ", Guid.NewGuid()), code, type);

			_assetManager.Register(shader);
			_storedShaders.Add(shader);

			Silk.NET.OpenGL.ShaderType oglEnum = Enum.Parse<Silk.NET.OpenGL.ShaderType>(shader.ShaderType.ToString());

			shader.Id = (int)_gl.CreateShader(oglEnum);
			_gl.ShaderSource((uint)shader.Id, shader.ShaderCode);
			_gl.CompileShader((uint)shader.Id);

			string shaderInfo = _gl.GetShaderInfoLog((uint)shader.Id);
			if (!string.IsNullOrEmpty(shaderInfo))
				_logger.LogError("Shader error {ShaderInfo}", shaderInfo); //Log errors.
			else
				_logger.LogInformation("{ShaderName} compiled correctly", shaderName);
		}
		else
		{
			_storedShaders.Add(registeredShader);
		}
	}

	public void LoadShaderFromFile(string filename, string shaderName, ShaderType type)
	{
		throw new NotImplementedException();
	}

	public void LinkShadersToProgram()
	{
		foreach (Shader storedShader in _storedShaders)
		{
			_gl.AttachShader((uint)ShaderProgramId, (uint)storedShader.Id);
		}

		_gl.LinkProgram((uint)ShaderProgramId);

		_attributeInfos = GetAttributesInformation((uint)ShaderProgramId);
		_uniforms = GetUniformsInformation((uint)ShaderProgramId);

		//After linking program we can detach shaders.
		foreach (Shader storedShader in _storedShaders)
		{
			_gl.DetachShader((uint)ShaderProgramId, (uint)storedShader.Id);
		}
	}

	public void UseProgram()
	{
		//throw new NotImplementedException();
	}

	public void GenBuffers()
	{
		for (int i = 0; i < _attributeInfos.Count; i++)
		{
			if (_buffers.ContainsKey(_attributeInfos.Values.ElementAt(i).Name)) continue;

			_gl.GenBuffers(1, out uint buffer);
			_buffers.Add(_attributeInfos.Values.ElementAt(i).Name, buffer);
		}

		for (int i = 0; i < _uniforms.Count; i++)
		{
			if (_buffers.ContainsKey(_uniforms.Values.ElementAt(i).Name)) continue;

			_gl.GenBuffers(1, out uint buffer);
			_buffers.Add(_uniforms.Values.ElementAt(i).Name, buffer);
		}
	}

	public void EnableVertexAttribArrays()
	{
		foreach (KeyValuePair<string, AttributeInfo> attributeInfo in _attributeInfos)
		{
			_gl.EnableVertexAttribArray((uint)attributeInfo.Value.Address);
		}
	}

	public void DisableVertexAttribArrays()
	{
		//throw new NotImplementedException();
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

	public int GetUniformAddress(string name)
	{
		_uniforms.TryGetValue(name, out UniformInfo result);
		return result?.Address ?? -1;
	}

	public uint GetBuffer(string name)
	{
		return _buffers.ContainsKey(name) ? _buffers[name] : 0;
	}
	
	private static Dictionary<string, AttributeInfo> GetAttributesInformation(uint shaderProgramId)
	{
		_gl.GetProgram(shaderProgramId, GLEnum.ActiveAttributes, out int attributeCount);
		Dictionary<string, AttributeInfo> attributes = new Dictionary<string, AttributeInfo>();
		for (uint i = 0; i < attributeCount; i++)
		{
			AttributeInfo info = new AttributeInfo();
			_gl.GetActiveAttrib(shaderProgramId, i, 256, out uint length, out info.Size, out AttributeType type, out string name);

			info.Name = name;
			info.Address = _gl.GetAttribLocation(shaderProgramId, info.Name);
			info.Type = (int)type;
			attributes.Add(name, info);
		}

		return attributes;
	}

	private static Dictionary<string, UniformInfo> GetUniformsInformation(uint shaderProgramId)
	{
		_gl.GetProgram(shaderProgramId, GLEnum.ActiveUniforms, out int uniformCount);
		Dictionary<string, UniformInfo> uniforms = new Dictionary<string, UniformInfo>();
		for (uint i = 0; i < uniformCount; i++)
		{
			UniformInfo info = new UniformInfo();
			_gl.GetActiveUniform(shaderProgramId, i, 256, out uint length, out info.Size, out UniformType type, out string name);

			info.Name = name;
			info.Address = _gl.GetUniformLocation(shaderProgramId, info.Name);
			info.Type = (int)type;
			uniforms.Add(name, info);
		}

		return uniforms;
	}
}