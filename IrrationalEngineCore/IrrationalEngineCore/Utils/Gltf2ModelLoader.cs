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
                //Only one mesh currently supported.
                Mesh[] meshes = deserializedFile.Meshes;
                

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    bufferBytes = deserializedFile.LoadBinaryBuffer(i, path);
                    int indecesAttributevalue = meshes[0].Primitives[0].Indices ?? 0;
                    int indecesBufferView = deserializedFile.Accessors[indecesAttributevalue].BufferView ?? 0;
                    int[] indeces = 
                        ParseBufferViews(bufferBytes, deserializedFile.Accessors[indecesAttributevalue],
                        deserializedFile.BufferViews[indecesBufferView]);
                    List<Vector3> vertexCoords = new List<Vector3>();
                    List<Vector3> normalCoords = new List<Vector3>();
                    List<Vector2> uvCoords = new List<Vector2>();
                    if (meshes[0].Primitives[0].Attributes.ContainsKey("POSITION"))
                    {
                        int attributeValue = meshes[0].Primitives[0].Attributes["POSITION"];
                        int bufferViewValue = deserializedFile.Accessors[attributeValue].BufferView ?? 0;
                        vertexCoords = ParseBufferViews(bufferBytes, 
                            deserializedFile.Accessors[attributeValue],
                            deserializedFile.BufferViews[bufferViewValue]);
                    }

                    if (meshes[0].Primitives[0].Attributes.ContainsKey("NORMAL"))
                    {
                        int attributeValue = meshes[0].Primitives[0].Attributes["NORMAL"];
                        int bufferViewValue = deserializedFile.Accessors[attributeValue].BufferView ?? 0;
                        normalCoords = ParseBufferViews(bufferBytes,
                            deserializedFile.Accessors[attributeValue],
                            deserializedFile.BufferViews[bufferViewValue]);
                    }

                    if (meshes[0].Primitives[0].Attributes.ContainsKey("TEXCOORD_0"))
                    {
                        int attributeValue = meshes[0].Primitives[0].Attributes["TEXCOORD_0"];
                        int bufferViewValue = deserializedFile.Accessors[attributeValue].BufferView ?? 0;
                        uvCoords = ParseBufferViews(bufferBytes,
                            deserializedFile.Accessors[attributeValue],
                            deserializedFile.BufferViews[bufferViewValue]);
                    }

                    Core.Entities.Mesh loadedModel = new Core.Entities.Mesh();

                    List<Vector3> decodedVertices = new List<Vector3>();
                    List<Vector3> decodedNormals = new List<Vector3>();
                    List<Vector2> decodedUvCoords = new List<Vector2>();



                    for (int j = 0; j < indeces.Length; j++)
                    {
                        decodedVertices.Add(vertexCoords.Count() == 0 ? Vector3.Zero : vertexCoords[indeces[j]]);
                        decodedNormals.Add(normalCoords.Count() == 0 ? Vector3.Zero : normalCoords[indeces[j]]);
                        decodedUvCoords.Add(uvCoords.Count() == 0 ? Vector2.Zero : uvCoords[indeces[j]]);
                    }

                    loadedModel.Vertices = decodedVertices.ToArray();
                    loadedModel.Normals = decodedVertices.ToArray();
                    loadedModel.UvCoords = decodedUvCoords.ToArray();
                    loadedModel.Indeces = indeces;

                    return loadedModel;
                }
            }
            catch (Exception e)
            {
                throw new Exception(path, e);
            }
            return null;
        }

        private dynamic ParseBufferViews(byte[] bufferBytes, Accessor accessors, BufferView bufferViews)
        {           
            if (accessors.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_SHORT && accessors.Type == Accessor.TypeEnum.SCALAR)
            {
                byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
               
                var size = subarray.Count() / sizeof(ushort);
                int[] ints = new int[size];
                    for (var index = 0; index < size; index++)
                    {
                        ints[index] = (int)BitConverter.ToUInt16(subarray, index * sizeof(ushort));
                    }
                return ints;
            }

             if (accessors.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_INT && accessors.Type == Accessor.TypeEnum.SCALAR)
            {
                byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
               
                var size = subarray.Count() / sizeof(uint);
                 int[] ints = new int[size];
                    for (var index = 0; index < size; index++)
                    {
                        ints[index] = (int)BitConverter.ToUInt32(subarray, index * sizeof(uint));
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
                    return vectors.ToList();
                }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC2)
            {
                byte[] subarray = SubArray(bufferBytes, bufferViews.ByteOffset, bufferViews.ByteLength);
            
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
                return vectors.ToList();
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
