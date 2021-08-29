using IeCoreEntities;
using IeCoreInterfaces.AssetImporters;
using IeCoreInterfaces.Assets;
using Microsoft.Extensions.Logging;
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

		private readonly ILogger<AssetManager> _logger;

		public AssetManager(IFbxImporter fbxImporter, ITextureImporter textureImporter, ILogger<AssetManager> logger)
		{
			AssetImporters.Add(textureImporter);
			AssetImporters.Add(fbxImporter);
			_logger = logger;
		}

		public void Register(Asset asset)
		{
			if (RegisteredAssets.TryAdd(asset.Name, asset))
			{
				_logger.LogDebug($"---------------------------------\nNew asset registered {asset.Name}");
				foreach (var registeredAsset in RegisteredAssets)
				{
					_logger.LogDebug($"-- Registered asset:{registeredAsset.Key} is type of {registeredAsset.Value.GetType()}");
				}
			}
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
						Register(importer.Import(file));
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
