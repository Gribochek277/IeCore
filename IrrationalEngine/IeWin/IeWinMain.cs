using AutoMapper;
using IeCoreEntities;
using IeCoreEntities.Model;
using IeWin.AssetImporters;
using IeWin.MappingProfiles;
using Newtonsoft.Json;

namespace IeWin;

/// <summary>
/// Platform-dependant project for reading and processing all
/// windows related data. Should not be used as dependency in main project and should be used as temporary solution or loose dependency.
/// </summary>
public class IeWinMain
{
	private static void Main()
	{
		var config = new MapperConfiguration(x =>
		{
			x.AddProfile(new AnimationProfile());
			x.AddProfile(new BoneProfile());
			x.AddProfile(new MeshProfile());
		});
		IFbxImporter importer = new FbxImporter(new Mapper(config));
		Asset? asset = importer.Import($"Resources{Path.DirectorySeparatorChar}FBX{Path.DirectorySeparatorChar}knight.fbx");
		
		string json = JsonConvert.SerializeObject((Model)asset); 
		File.WriteAllText("./knight.json", json);
	}
}