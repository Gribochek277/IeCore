namespace IeCoreEntities.Shaders
{
	/// <summary>
	/// Determines shader asset.
	/// </summary>
	public class Shader : Asset
	{
		/// <summary>
		/// Shader ID.
		/// </summary>
		public int Id = -1;
		/// <summary>
		/// Shader source code
		/// </summary>
		public string ShaderCode { get; private set; }
		/// <summary>
		/// Shader type. <see cref="Shaders.ShaderType"/>
		/// </summary>
		public ShaderType ShaderType { get; private set; }

		/// <summary>
		/// Ctor. <see cref="Asset"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="file"></param>
		/// <param name="code"></param>
		/// <param name="type"></param>
		public Shader(string name, string file, string code, ShaderType type) : base(name, file)
		{
			ShaderCode = code;
			ShaderType = type;
		}
	}
}
