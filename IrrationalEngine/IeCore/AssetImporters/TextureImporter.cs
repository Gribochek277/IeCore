using IeCoreEntities;
using IeCoreEntities.Materials;
using IeCoreInterfaces.AssetImporters;
using System;

namespace IeCore.AssetImporters
{
	public class TextureImporter : ITextureImporter
	{
		public Type AssetType => typeof(Texture);

		public string[] FileExtensions { get; } = { ".png", ".jpg", "jpeg" };

		public Asset Import(string file)
		{
			throw new NotImplementedException();
		}
	}
}
