using IeCoreEntites;
using IeCoreInterfaces.AssetImporters;
using System.Collections.Generic;

namespace IeCoreInterfaces.Assets
{
    /// <summary>
    /// Contains all assets in one place, prevents loading of duplicate assets.
    /// </summary>
    public interface IAssetManager
    {
        /// <summary>
        /// List of asset importers
        /// which will be used in <see cref="RegisterFile{T}(string)"/>.
        /// </summary>
        List<IAssetImporter> AssetImporters { get; }
        /// <summary>
        /// Dictionary of registered assets.
        /// </summary>
        Dictionary<string, Asset> RegisteredAssets { get; }
        /// <summary>
        /// Import asset from file.
        /// </summary>
        /// <typeparam name="T">asset type</typeparam>
        /// <param name="file"></param>
        void RegisterFile<T>(string file);

        /// <summary>
        /// Import asset from file.
        /// </summary>
        /// <param name="file"></param>
        void RegisterFile(string file);
        /// <summary>
        /// Registers asset to prevent duplication.
        /// </summary>
        /// <param name="asset"></param>
        void Register(Asset asset);
        /// <summary>
        /// Get asset by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Retrieve<T>(string name) where T : Asset;
        /// <summary>
        /// Get asset by file name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        T RetrieveFile<T>(string file) where T : Asset;
        /// <summary>
        /// Get all assets of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> RetrieveAll<T>() where T : Asset;
    }
}
