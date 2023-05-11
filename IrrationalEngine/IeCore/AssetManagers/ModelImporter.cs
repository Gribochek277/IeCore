using System;
using IeCoreEntities;
using IeCoreInterfaces.AssetImporters;

namespace IeCore.AssetManagers;

public class ModelImporter: IModelImporter
{
	public Type AssetType { get; }
	public string[] FileExtensions { get; }
	public Asset Import(string file)
	{
		throw new NotImplementedException();
	}
}