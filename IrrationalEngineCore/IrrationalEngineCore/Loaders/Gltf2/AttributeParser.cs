using glTFLoader.Schema;
using IrrationalEngineCore.Loaders.Gltf2.Gltf2Extentions;
using IrrationalEngineCore.Loaders.Gltf2.Interfaces;

namespace IrrationalEngineCore.Loaders.Gltf2
{
    internal class AttributeParser : IAttributesParser
    {
        private readonly string _attribName;
        private int _meshIndex;
        private int _primitiveIndex;
        private Gltf _deserializedFile;
        private byte[] _bufferBytes;
        internal dynamic ParsedAttribute { get; private set; }

        public AttributeParser(int meshIndex, int primitiveIndex, Gltf deserializedFile, byte[] bufferBytes, string attributeName)
        {
            _meshIndex = meshIndex;
            _deserializedFile = deserializedFile;
            _bufferBytes = bufferBytes;
            _primitiveIndex = primitiveIndex;
            _attribName = attributeName;
        }
        public bool Parse()
        {
            if (_deserializedFile.Meshes[_meshIndex].Primitives[_primitiveIndex].Attributes.ContainsKey(_attribName))
            {
                int attributeValue = _deserializedFile.Meshes[_meshIndex].Primitives[_primitiveIndex].Attributes[_attribName];
                int bufferViewValue = _deserializedFile.Accessors[attributeValue].BufferView ?? 0;
                ParsedAttribute = _bufferBytes.ParseBufferViews(
                    _deserializedFile.Accessors[attributeValue],
                    _deserializedFile.BufferViews[bufferViewValue]);
                return true;
            }
            return false;
        }       
    }
}
