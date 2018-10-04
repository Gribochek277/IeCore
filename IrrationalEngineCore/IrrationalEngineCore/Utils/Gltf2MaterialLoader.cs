using System;
using System.Collections.Generic;
using System.IO;
using glTFLoader;
using Irrational.Core.Entities;
using Irrational.Utils.Interfaces;

namespace Irrational.Utils
{
    class Gltf2MaterialLoader : IMaterialLoader
    {
        public Dictionary<string, Material> LoadFromFile(string path)
        {
            Dictionary<string, Material> mats = new Dictionary<string, Material>();
            string relativeLocation = Directory.GetParent(path).FullName;
            try
            {
                var deserializedFile = Interface.LoadModel(path);
                foreach (glTFLoader.Schema.Material material in deserializedFile.Materials)
                {
                    Material mat = new Material();
                    mat.MaterialName = material.Name;
                    mat.DiffuseMap = Path.Combine(relativeLocation,deserializedFile.Images[material.PbrMetallicRoughness.BaseColorTexture.Index].Uri);
                    mat.NormalMap =  Path.Combine(relativeLocation,deserializedFile.Images[material.NormalTexture.Index].Uri);
                    mat.MetallicRoughness =  Path.Combine(relativeLocation,deserializedFile.Images[material.PbrMetallicRoughness.MetallicRoughnessTexture.Index].Uri);
                    try{
                    mat.AmbientMap = Path.Combine(relativeLocation,deserializedFile.Images[material.OcclusionTexture.Index].Uri);
                    }
                    catch{mat.AmbientMap = mat.MetallicRoughness;}
                    try{
                        mat.NormalScale = material.NormalTexture.Scale;
                    }
                    catch
                    {}
                    mats.Add(material.Name, mat);
                }
            }            
            catch (Exception e)
            {
                throw new Exception(path, e);
            }    

          return mats;
        }
    }
}
