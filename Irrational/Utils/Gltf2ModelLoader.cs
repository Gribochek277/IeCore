using glTFLoader;
using glTFLoader.Schema;
using Irrational.Utils.Interfaces;
using OpenTK;
using System;
using System.IO;

namespace Irrational.Utils
{
    public class Gltf2ModelLoader : IModelLoader
    {
        public Core.Entities.Mesh LoadFromFile(string path)
        {

            if (!path.EndsWith("gltf") && !path.EndsWith("glb")) return null;

            try
            {
                var deserializedFile = Interface.LoadModel(path);

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    var bufferBytes = deserializedFile.LoadBinaryBuffer(i, path);
                }

                // open all images
                for (int i = 0; i < deserializedFile.Images?.Length; ++i)
                {
                    using (var s = deserializedFile.OpenImageFile(i, path))
                    {
                        using (var rb = new BinaryReader(s))
                        {
                            uint header = rb.ReadUInt32();

                            if (header == 0x474e5089) continue; // PNG
                            if ((header & 0xffff) == 0xd8ff) continue; // JPEG  
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception(path, e);
            }            
        }

        public Core.Entities.Mesh LoadFromString(string objModel)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
