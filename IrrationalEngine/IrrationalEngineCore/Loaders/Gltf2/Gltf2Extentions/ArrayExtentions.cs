using glTFLoader.Schema;
using IrrationalEngineCore.Utils.Extentions;
using OpenTK;
using System;
using System.Linq;

namespace IrrationalEngineCore.Loaders.Gltf2.Gltf2Extentions
{
    public static class ArrayExtentions
    {
        public static dynamic ParseBufferViews(this byte[] bufferBytes, Accessor accessors, BufferView bufferViews)
        {
            if (accessors.ComponentType == Accessor.ComponentTypeEnum.UNSIGNED_SHORT && accessors.Type == Accessor.TypeEnum.SCALAR)
            {
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

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
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

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
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

                int[] ints = new int[subarray.Length];
                for (var index = 0; index < subarray.Length; index++)
                {
                    ints[index] = subarray[index];
                }

                return ints;
            }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC3)
            {
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

                var size = subarray.Count() / sizeof(float);
                var floats = new float[size];
                for (var index = 0; index < size; index++)
                {
                    floats[index] = BitConverter.ToSingle(subarray, index * sizeof(float));
                }
                Vector3[] vectors = new Vector3[floats.Count() / 3];
                for (int f = 0; f < floats.Count(); f += 3)
                {
                    vectors[f / 3] = new Vector3(floats[f], floats[f + 1], floats[f + 2]);
                }
                return vectors.ToList();
            }

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC2)
            {
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

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

            if (accessors.ComponentType == Accessor.ComponentTypeEnum.FLOAT && accessors.Type == Accessor.TypeEnum.VEC4)
            {
                byte[] subarray = bufferBytes.SubArray(bufferViews.ByteOffset, bufferViews.ByteLength);

                var size = subarray.Count() / sizeof(float);
                var floats = new float[size];
                for (var index = 0; index < size; index++)
                {
                    floats[index] = BitConverter.ToSingle(subarray, index * sizeof(float));
                }
                Vector4[] vectors = new Vector4[floats.Count() / 4];
                for (int f = 0; f < floats.Count(); f += 4)
                {
                    vectors[f / 4] = new Vector4(floats[f], floats[f + 1], floats[f + 2], floats[f + 3]);
                }
                return vectors.ToList();
            }

            return null;
        }
    }
}
