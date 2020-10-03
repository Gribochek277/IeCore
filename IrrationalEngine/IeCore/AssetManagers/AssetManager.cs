using IeCore.AssetImporters;
using IeCoreEntites;
using IeCoreInterfaces.AssetImporters;
using IeCoreInterfaces.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IeCore.AssetManagers
{
    public class AssetManager : IAssetManager
    {
        public List<IAssetImporter> AssetImporters { get; } = new List<IAssetImporter>();

        public Dictionary<string, Asset> RegisteredAssets { get; } = new Dictionary<string, Asset>();

        public AssetManager()
        {
            AssetImporters.Add(new TextureImporter());
            AssetImporters.Add(new FbxImporter());
        }

        public void Register(Asset asset)
        {
            RegisteredAssets.TryAdd(asset.Name, asset);
        }

        public void RegisterFile<T>(string file)
        {
            
        }

        public void RegisterFile(string file)
        {
            string fileExtention = Path.GetExtension(file);
            foreach (IAssetImporter importer in AssetImporters)
            {
                foreach (string importerFileExtention in importer.FileExtensions)
                {
                    if (importerFileExtention == fileExtention)
                    {
                        importer.Import(file);
                        return;
                    }
                }
            }
        }

        public T Retrieve<T>(string name) where T : Asset
        {
            RegisteredAssets.TryGetValue(name, out Asset asset);
            return (T)asset;
        }

        public IEnumerable<T> RetrieveAll<T>() where T : Asset
        {
            return RegisteredAssets.Values.Where(asset => asset.GetType() == typeof(T)).Cast<T>();
        }

        public T RetrieveFile<T>(string file) where T : Asset
        {
            throw new NotImplementedException();
        }
    }
}
