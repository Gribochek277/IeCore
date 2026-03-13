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

		public AssetManager(IModelImporter modelImporter, ITextureImporter textureImporter, ILogger<AssetManager> logger)
		{
			AssetImporters.Add(textureImporter);
			AssetImporters.Add(modelImporter);
			_logger = logger;
		}

		public void Register(Asset asset)
		{
			if (RegisteredAssets.TryAdd(asset.Name, asset))
			{
				_logger.LogDebug("---------------------------------\\nNew asset registered {AssetName}", asset.Name);
				foreach (var registeredAsset in RegisteredAssets)
				{
					_logger.LogDebug("-- Registered asset:{RegisteredAssetKey} is type of {Type}", registeredAsset.Key, registeredAsset.Value.GetType());
				}
			}
		}

		public void RegisterFile<T>(string file)
		{

		}

		public void RegisterFile(string file)
		{
			file = ResolveFilePath(file);
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

		private string ResolveFilePath(string file)
		{
			if (Path.IsPathRooted(file) && File.Exists(file))
			{
				return file;
			}

			if (File.Exists(file))
			{
				return Path.GetFullPath(file);
			}

			string fromBaseDirectory = Path.Combine(AppContext.BaseDirectory, file);
			if (File.Exists(fromBaseDirectory))
			{
				return fromBaseDirectory;
			}

			string normalizedFile = file.Replace('/', Path.DirectorySeparatorChar)
				.Replace('\\', Path.DirectorySeparatorChar);
			int resourcesIndex = normalizedFile.IndexOf($"Resources{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
			if (resourcesIndex >= 0)
			{
				string resourceRelativePath = normalizedFile.Substring(resourcesIndex + "Resources".Length + 1);
				string resolved = TryResolveUnderKnownProjects(resourceRelativePath);
				if (!string.IsNullOrEmpty(resolved))
				{
					return resolved;
				}
			}

			throw new FileNotFoundException($"Asset file was not found: '{file}'.", file);
		}

		private static string TryResolveUnderKnownProjects(string resourceRelativePath)
		{
			string current = Directory.GetCurrentDirectory();
			while (!string.IsNullOrEmpty(current))
			{
				string ieCoreCandidate = Path.Combine(current, "IrrationalEngine", "IeCore", "Resources", resourceRelativePath);
				if (File.Exists(ieCoreCandidate))
				{
					return ieCoreCandidate;
				}

				string ieUtilsCandidate = Path.Combine(current, "IrrationalEngine", "IeUtils", "Resources", resourceRelativePath);
				if (File.Exists(ieUtilsCandidate))
				{
					return ieUtilsCandidate;
				}

				DirectoryInfo parent = Directory.GetParent(current);
				current = parent == null ? string.Empty : parent.FullName;
			}

			return string.Empty;
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
