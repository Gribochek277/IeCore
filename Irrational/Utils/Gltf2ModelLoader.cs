using glTFLoader;
using glTFLoader.Schema;
using Irrational.Utils.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                byte[] bufferBytes = null;

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    bufferBytes = deserializedFile.LoadBinaryBuffer(i, path);
                    int[] indeces = ParseBufferViews(bufferBytes, deserializedFile.Accessors[0], deserializedFile.BufferViews[0]);
                    Vector3[] vertexCoords = ParseBufferViews(bufferBytes, deserializedFile.Accessors[1], deserializedFile.BufferViews[1]);
                    Vector3[] normalCoords = ParseBufferViews(bufferBytes, deserializedFile.Accessors[2], deserializedFile.BufferViews[2]);
                    // var uvCoords = ParseBufferViews(bufferBytes, deserializedFile.Accessors[3], deserializedFile.BufferViews[3]);

                    Core.Entities.Mesh loadedModel = new Core.Entities.Mesh();
                    // loadedModel.normals = normalCoords;
                    //  loadedModel.uvCoords = uvCoords;

                    List<Vector3> decodedVertices = new List<Vector3>();
                    List<Vector3> decodedNormals = new List<Vector3>();

                    

                    for (int j = 0; j < indeces.Length; j++)
                    {
                        decodedVertices.Add(vertexCoords[indeces[j]]);
                        decodedNormals.Add(vertexCoords[indeces[j]]);
                    }

                    loadedModel.vertices = decodedVertices.ToArray();
                    loadedModel.normals = decodedVertices.ToArray();
                    loadedModel.indeces = indeces;

                    return loadedModel;
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

        private dynamic ParseBufferViews(byte[] bufferBytes, Accessor accessors, BufferView bufferViews)
        {
           
                if (accessors.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_SHORT && accessors.Type == Accessor.TypeEnum.SCALAR)
                {
                    byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
                    ushort[] ushortBuffer = subarray.Select(x => (ushort)x).ToArray();

                    var size = subarray.Count() / sizeof(ushort);
                    int[] ints = new int[size];
                        for (var index = 0; index < size; index++)
                        {
                            ints[index] = (int)BitConverter.ToUInt16(subarray, index * sizeof(ushort));
                        }
                    return ints;
                }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_BYTE && accessors.Type == Accessor.TypeEnum.SCALAR)
            {
                byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);

                int[] ints = new int[subarray.Length];
                for (var index = 0; index < subarray.Length; index++)
                {
                    ints[index] = subarray[index];
                }

                return ints;
            }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC3)
                {
                    byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
                    float[] ushortBuffer = subarray.Select(x => (float)x).ToArray();

                    var size = subarray.Count() / sizeof(float);
                    var floats = new float[size];
                    for (var index = 0; index < size; index++)
                    {
                        floats[index] = BitConverter.ToSingle(subarray, index * sizeof(float));
                    }
                    Vector3[] vectors = new Vector3[floats.Count() / 3];
                    for (int f = 0; f < floats.Count(); f += 3)
                    {
                        vectors[f / 3] = new Vector3(floats[f],floats[f + 1], floats[f + 2]);
                    }
                    return vectors;
                }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC2)
            {
                byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
                float[] ushortBuffer = subarray.Select(x => (float)x).ToArray();

                var size = subarray.Count() / sizeof(float);
                var floats = new float[size];
                for (var index = 0; index < size; index++)
                {
                    floats[index] = BitConverter.ToSingle(subarray, index * sizeof(float));
                }
                Vector2[] vectors = new Vector2[floats.Count() / 2];
                for (int f = 0; f < floats.Count(); f += 2)
                {
                    vectors[f / 2] = new Vector2(floats[f], floats[f + 1]);
                }
                return vectors;
            }
            return null;
        }


        public T[] SubArray<T>(T[] data, int index, int length) // may be required deep clone in future
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
