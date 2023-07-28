using IeCoreEntities.Shaders;
using IeCoreInterfaces.Shaders;

namespace IeCoreSilkNetOpenGl.Shaders;

public class ShaderProgram: IShaderProgram
{
	
	private bool _disposedValue;
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

	public int ShaderProgramId { get; }
	public void LoadShaderFromString(string code, string shaderName, ShaderType type)
	{
		//throw new NotImplementedException();
	}

	public void LoadShaderFromFile(string filename, string shaderName, ShaderType type)
	{
		//throw new NotImplementedException();
	}

	public void LinkShadersToProgram()
	{
		//throw new NotImplementedException();
	}

	public void UseProgram()
	{
		//throw new NotImplementedException();
	}

	public void GenBuffers()
	{
		//throw new NotImplementedException();
	}

	public void EnableVertexAttribArrays()
	{
		//throw new NotImplementedException();
	}

	public void DisableVertexAttribArrays()
	{
		//throw new NotImplementedException();
	}

	public int GetAttributeAddress(string name)
	{
		throw new NotImplementedException();
	}

	public int GetUniformAddress(string name)
	{
		
		return 1;
		//throw new NotImplementedException();
	}

	public uint GetBuffer(string name)
	{
		return 1;
		//throw new NotImplementedException();
	}
}