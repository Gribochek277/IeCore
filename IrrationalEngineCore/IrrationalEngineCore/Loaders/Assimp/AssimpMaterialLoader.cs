using Assimp;
using Irrational.Loaders.Interfaces;
using System;
using System.Collections.Generic;
using Mesh = Assimp.Mesh;
using AssimpMat = Assimp.Material;
using EngineMaterial = Irrational.Core.Entities.Material;
using Scene = Assimp.Scene;
using System.IO;
using System.Linq;

namespace IrrationalEngineCore.Loaders.Assimp
{
    internal class AssimpMaterialLoader : IMaterialLoader
    {
        public Dictionary<string, EngineMaterial> LoadFromFile(string path)
        {
            string relativeLocation = Directory.GetParent(path).FullName;

            Dictionary<string, EngineMaterial> mats = new Dictionary<string, EngineMaterial>();
            List<Mesh> meshes = new List<Mesh>();

            AssimpContext context = new AssimpContext();
            Scene loadedAssimpScene = context.ImportFile(path, PostProcessSteps.OptimizeMeshes | PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);
            if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
            {
                Console.WriteLine("Scene error");
            }

            List<AssimpMat> materials = loadedAssimpScene.Materials;

            foreach (var material in materials)
            {
                EngineMaterial _material = new EngineMaterial();
                _material.MaterialName = material.Name;
                TextureSlot[] textureSlots =  material.GetAllMaterialTextures();
                if (material.HasTextureDiffuse)
                {
                    _material.DiffuseMap = Path.Combine(relativeLocation, material.TextureDiffuse.FilePath);
                }

                if (material.HasTextureNormal)
                {
                    _material.NormalMap = Path.Combine(relativeLocation, material.TextureNormal.FilePath);
                }

                if (material.HasTextureAmbient)
                {
                    _material.AmbientMap = Path.Combine(relativeLocation, material.TextureAmbient.FilePath);
                }

                _material.MetallicRoughness = Path.Combine(relativeLocation, textureSlots.Where(t=>t.TextureType == TextureType.Unknown).FirstOrDefault().FilePath);

                mats.Add(_material.MaterialName, _material);
            }

            return mats;
        }
    }
}
