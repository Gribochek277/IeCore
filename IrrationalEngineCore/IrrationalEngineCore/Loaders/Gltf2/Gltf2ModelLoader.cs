using glTFLoader;
using glTFLoader.Schema;
using Irrational.Loaders.Interfaces;
using IrrationalEngineCore.Loaders.Gltf2;
using IrrationalEngineCore.Loaders.Gltf2.Gltf2Extentions;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Irrational.Loaders.Gltf2
{
    public class Gltf2ModelLoader : IModelLoader
    {
        public Core.Entities.Mesh LoadFromFile(string path)
        {

            if (!path.EndsWith("gltf") && !path.EndsWith("glb")) return null;

            try
            {
                Gltf deserializedFile = Interface.LoadModel(path);
                byte[] bufferBytes = null;
                //Only one mesh currently supported.
                Mesh[] meshes = deserializedFile.Meshes;

                Core.Entities.Mesh loadedModel = new Core.Entities.Mesh();

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; i++)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    bufferBytes = deserializedFile.LoadBinaryBuffer(i, path);
                    int indecesAttributevalue = meshes[0].Primitives[0].Indices ?? 0;
                    int indecesBufferView = deserializedFile.Accessors[indecesAttributevalue].BufferView ?? 0;
                    int[] indeces =
                        bufferBytes.ParseBufferViews(deserializedFile.Accessors[indecesAttributevalue],
                        deserializedFile.BufferViews[indecesBufferView]);
                    foreach (KeyValuePair<string,int> attribute in deserializedFile.Meshes[0].Primitives[0].Attributes)
                    {
                        Console.WriteLine(attribute.Key);
                    }

                    AttributeParser posParser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "POSITION");
                    AttributeParser normParser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "NORMAL");
                    AttributeParser uv0Parser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "TEXCOORD_0");
                    AttributeParser uv1Parser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "TEXCOORD_1");
                    AttributeParser tangentParser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "TANGENT");

                    //Animations
                    AttributeParser jointsParser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "JOINTS_0");
                    AttributeParser weightsParser = new AttributeParser(0, 0, deserializedFile, bufferBytes, "WEIGHTS_0");

                    List<Vector3> vertexCoords = posParser.Parse() ? posParser.ParsedAttribute : null;
                    List<Vector3> normalCoords = normParser.Parse() ? normParser.ParsedAttribute : null;
                    List<Vector2> uv0Coords = uv0Parser.Parse() ? uv0Parser.ParsedAttribute : null;
                    List<Vector2> uv1Coords = uv1Parser.Parse() ? uv1Parser.ParsedAttribute : null;
                    List<Vector4> tangentCoords = tangentParser.Parse() ? tangentParser.ParsedAttribute : null;

                    //Animations
                    List<Vector4> jointsValues = jointsParser.Parse() ? jointsParser.ParsedAttribute : null;
                    List<Vector4> weightsValues = weightsParser.Parse() ? weightsParser.ParsedAttribute : null;

                    List<Vector3> decodedVertices = new List<Vector3>();
                    List<Vector3> decodedNormals = new List<Vector3>();
                    List<Vector2> decodedUvCoords = new List<Vector2>();


                    for (int j = 0; j < indeces.Length; j++)
                    {
                        decodedVertices.Add(vertexCoords.Count() == 0 ? Vector3.Zero : vertexCoords[indeces[j]]);
                        decodedNormals.Add(normalCoords.Count() == 0 ? Vector3.Zero : normalCoords[indeces[j]]);
                        decodedUvCoords.Add(uv0Coords.Count() == 0 ? Vector2.Zero : uv0Coords[indeces[j]]);
                    }

                    loadedModel.Vertices = decodedVertices.ToArray();
                    loadedModel.Normals = decodedVertices.ToArray();
                    loadedModel.UvCoords = decodedUvCoords.ToArray();
                    loadedModel.Indeces = indeces;
                }
                return loadedModel;
            }
            catch (Exception e)
            {
                throw new Exception(path, e);
            }
        }
    }
}
