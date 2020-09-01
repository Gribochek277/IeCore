using IeCoreEntites;
using IeCoreInterfaces.AssetImporters;
using IeCoreInterfaces.Assets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IeCore.AssetManagers
{
    public class AssetManager : IAssetManager
    {
        public Dictionary<string, IAssetImporter> AssetImporters { get; } = new Dictionary<string, IAssetImporter>();

        public Dictionary<string, Asset> RegisteredAssets { get; } = new Dictionary<string, Asset>();

        public void Register(Asset asset)
        {
            RegisteredAssets.TryAdd(asset.Name, asset);
        }

        public void RegisterFile<T>(string file)
        {
            throw new NotImplementedException();
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
