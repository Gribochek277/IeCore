using System.Collections.Generic;
using System.IO;
using glTFLoader;
using glTFLoader.Schema;
using IrrationalEngineCore.Loaders.Interfaces;
using Material = IrrationalEngineCore.Core.Entities.Material;

namespace IrrationalEngineCore.Loaders.Gltf2
{
    internal class Gltf2MaterialLoader : IMaterialLoader
    {
        private const string DefaultTexture = "Resources/Textures/FallbackTexture.jpg";
        public Dictionary<string, Material> LoadFromFile(string path)
        {
            Dictionary<string, Material> mats = new Dictionary<string, Material>();
            string relativeLocation = Directory.GetParent(path).FullName;

            Gltf deserializedFile = Interface.LoadModel(path);
            foreach (glTFLoader.Schema.Material material in deserializedFile.Materials)
            {
                Material mat = new Material();
                mat.MaterialName = material.Name ?? "NewMaterial";

                int diffuseMap = material.PbrMetallicRoughness != null && 
                    material.PbrMetallicRoughness.BaseColorTexture != null ? material.PbrMetallicRoughness.BaseColorTexture.Index : -1;

                int normalMap = material.NormalTexture != null ? material.NormalTexture.Index : -1;

                int metallicRoughness = material.PbrMetallicRoughness != null && 
                    material.PbrMetallicRoughness.MetallicRoughnessTexture != null ? material.PbrMetallicRoughness.MetallicRoughnessTexture.Index : -1;

                int ambientMap = material.OcclusionTexture != null ? material.OcclusionTexture.Index : -1;

                mat.DiffuseMap = GetTexturePathOrDefault(deserializedFile, relativeLocation, diffuseMap);
                mat.NormalMap = GetTexturePathOrDefault(deserializedFile,relativeLocation, normalMap);              
                mat.MetallicRoughness = GetTexturePathOrDefault(deserializedFile, relativeLocation, metallicRoughness);                   
                mat.AmbientMap = GetTexturePathOrDefault(deserializedFile, relativeLocation, ambientMap);
              
                
                mats.Add(mat.MaterialName, mat);
            }
           

            return mats;
        }

        private string GetTexturePathOrDefault(Gltf deserializedFile, string relativeLocation, int Index)
        {
            return deserializedFile.Images!= null && 
                deserializedFile.Images.Length > Index && Index > -1 ? 
                Path.Combine(relativeLocation, deserializedFile.Images[Index].Uri) :
                Path.Combine(relativeLocation, DefaultTexture);
        }
    }
}
