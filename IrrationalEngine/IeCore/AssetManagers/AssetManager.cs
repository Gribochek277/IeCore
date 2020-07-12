using IeCoreEntites;
using IeCoreInterfaces.AssetImporters;
using IeCoreInterfaces.Assets;
using System;
using System.Collections.Generic;

namespace IeCore.AssetManagers
{
    public class AssetManager : IAssetManager
    {
        public Dictionary<string, IAssetImporter> AssetImporters { get; } = new Dictionary<string, IAssetImporter>();

        public Dictionary<string, Asset> RegisteredAssets { get; } = new Dictionary<string, Asset>();

        public void Register(Asset asset)
        {
            RegisteredAssets.Add(asset.Name, asset);
        }

        public void RegisterFile<T>(string file)
        {
            throw new NotImplementedException();
        }

        public T Retrieve<T>(string name) where T : Asset
        {
            return (T)RegisteredAssets[name];
        }

        public IEnumerable<T> RetrieveAll<T>() where T : Asset
        {
            throw new NotImplementedException();
        }

        public T RetrieveFile<T>(string file) where T : Asset
        {
            throw new NotImplementedException();
        }
    }
}
