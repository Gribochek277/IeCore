using IeCoreEntites.Materials;
using IeCoreInterfaces.AssetImporters;
using System;

namespace IeCore.AssetImporters
{
    public class TextureImporter : IMaterialImporter
    {
        public Type AssetType => typeof(Texture);

        public string[] FileExtensions { get; } = new string[] { ".png", ".jpg", "jpeg" };

        public void Import(string file)
        {
            throw new NotImplementedException();
        }
    }
}
