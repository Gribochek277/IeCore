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
                    mat.DiffuseMap = relativeLocation +"//"+ deserializedFile.Images[0].Uri;
                    mat.NormalMap = relativeLocation + "//" + deserializedFile.Images[material.NormalTexture.Index].Uri;
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
