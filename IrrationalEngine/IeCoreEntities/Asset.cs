using IeUtils;

namespace IeCoreEntities
{
	/// <summary>
	/// Base class for all loaded assets. 
	/// </summary>
	public abstract class Asset
	{
		/// <summary>
		/// Asset name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// File from wich this asset was loaded.
		/// </summary>
		public string File { get; private set; }

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="file"></param>
		public Asset(string name, string file)
		{
			name.AssertNotNullorEmpty(nameof(name));
			file.AssertNotNullorEmpty(nameof(file));
			Name = name;
			File = file;
		}
	}
}
