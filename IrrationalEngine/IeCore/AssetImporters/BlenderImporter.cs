using Assimp;
using IeCoreEntites.Model;
using IeCoreInterfaces.AssetImporters;
using System;
using System.Collections.Generic;

namespace IeCore.AssetImporters
{
    public class BlenderImporter : IAssetImporter
    {
        public Type AssetType => typeof(Model);

        public string[] FileExtensions => new string[] { ".blend" };

        List<Assimp.Mesh> meshes = new List<Assimp.Mesh>();
        public void Import(string file)
        {
            AssimpContext context = new AssimpContext();

            const PostProcessSteps ASSIMP_LOAD_FLAGS = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
            Scene loadedAssimpScene = context.ImportFile(file, ASSIMP_LOAD_FLAGS);
            if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
            {
                Console.WriteLine("Scene error");
            }
        }
    }
}
