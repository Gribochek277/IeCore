using System;
using System.IO;
using IeCoreEntities;
using IeCoreEntities.Model;
using IeCoreInterfaces.AssetImporters;
using Newtonsoft.Json;

namespace IeCore.AssetImporters;

public class ModelImporter: IModelImporter
{
	public Type AssetType => typeof(Model);
	public string[] FileExtensions => new[] { ".json" };
	public Asset Import(string file)
	{
		string json =  File.ReadAllText(file);
		Asset model = JsonConvert.DeserializeObject<Model>(json);
		return model;
	}
}