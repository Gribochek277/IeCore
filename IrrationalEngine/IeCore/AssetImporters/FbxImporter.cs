using Assimp;
using AutoMapper;
using IeCoreEntites.Model;
using IeCoreInterfaces.AssetImporters;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace IeCore.AssetImporters
{
    public class FbxImporter : IAssetImporter
    {
        private IMapper _mapper;
        public Type AssetType => typeof(Model);

        public string[] FileExtensions => new string[] { ".fbx" };

        public FbxImporter()
        {
            _mapper = IrrationalEngine.ServiceProvider.GetService<IMapper>();
        }

        public void Import(string file)
        {
            using (AssimpContext context = new AssimpContext()) 
            { 
                const PostProcessSteps ASSIMP_LOAD_FLAGS = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
                Scene loadedAssimpScene = context.ImportFile(file, ASSIMP_LOAD_FLAGS);
                if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
                {
                    Console.WriteLine("Scene error");
                }

                Model model = new Model(Path.GetFileName(file), file);
                foreach(Assimp.Mesh assimpMesh in loadedAssimpScene.Meshes)
                    model.Meshes.Add(_mapper.Map<Assimp.Mesh, IeCoreEntites.Model.Mesh>(assimpMesh));
                Context.Assetmanager.Register(model);
            }
        }
    }
}
