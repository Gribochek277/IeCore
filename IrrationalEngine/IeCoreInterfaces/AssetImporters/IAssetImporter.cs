using System;
using IeCoreEntities;

namespace IeCoreInterfaces.AssetImporters
{
    /// <summary>
    /// Serves for importing assets of all types.
    /// </summary>
    public interface IAssetImporter
    {
        /// <summary>
        /// Determines type of asset.
        /// </summary>
        Type AssetType { get; }
        /// <summary>
        /// Array of possible file extensions for this type of asset.
        /// </summary>
        string[] FileExtensions { get; }
        /// <summary>
        /// Imports asset according to file path.
        /// </summary>
        /// <param name="file"></param>
        Asset Import(string file);
    }
}
