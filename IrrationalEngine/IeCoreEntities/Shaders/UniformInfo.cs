namespace IeCoreEntities.Shaders
{
	/// <summary>
	/// Contains information about shader uniform.
	/// </summary>
	public class UniformInfo
	{
		/// <summary>
		/// Uniform name.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Uniform address.
		/// </summary>
		public int Address { get; set; } = -1;
		/// <summary>
		/// Size of uniform.
		/// </summary>
		public int Size = 0;
		/// <summary>
		/// Code of uniform type as integer value.
		/// </summary>
		public int Type;
	}
}
