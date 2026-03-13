using IeCoreEntities.Animation;
using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class ModelComponent : IModelComponent
	{
		private const int MaxBonesPerVertex = 4;

		public string Name => "ModelSceneObjectComponent";

		public Model Model { get; private set; }

		private uint[] _indexes;
		private float[] _vboTextureData;
		private float[] _vboPositionData;
		private Matrix4x4[] _vboBonesDataOfModel;
		private int[] _boneIdsPerVertex;
		private float[] _boneWeightsPerVertex;

		public ModelComponent(Model model)
		{
			Model = model;
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
			Model = null;
		}

		public float[] GetVboPositionDataOfModel() //TODO: Add caching;
		{
			if (_vboPositionData != null) return _vboPositionData;
			var positionData = new List<float>();
			foreach (Mesh mesh in Model.Meshes)
			{
				foreach (Vertex vertex in mesh.Vertices)
				{
					positionData.Add(vertex.Position.X);
					positionData.Add(vertex.Position.Y);
					positionData.Add(vertex.Position.Z);
				}
			}

			_vboPositionData = positionData.ToArray();

			return _vboPositionData;
		}

		public float[] GetVboTextureDataOfModel() //TODO: Add caching;
		{
			if (_vboTextureData != null) return _vboTextureData;

			var textureData = new List<float>();
			foreach (Mesh mesh in Model.Meshes)
			{
				foreach (Vertex vertex in mesh.Vertices)
				{
					textureData.Add(vertex.TextureCoordinates.X);
					textureData.Add(vertex.TextureCoordinates.Y);
				}
			}

			_vboTextureData = textureData.ToArray();

			return _vboTextureData;
		}

		public Matrix4x4[] GetVboBonesDataOfModel() //TODO: Add caching;
		{
			if (_vboBonesDataOfModel != null) return _vboBonesDataOfModel;

			var bonesData = new List<Matrix4x4>();
			foreach (Mesh mesh in Model.Meshes)
			{
				foreach (Bone bone in mesh.Skeleton.Bones)
				{
					bonesData.Add(bone.OffsetMatrix);
				}
			}

			_vboBonesDataOfModel = bonesData.ToArray();
			return _vboBonesDataOfModel;
		}

		public uint[] GetIndexesOfModel() //TODO: Add caching;
		{
			if (_indexes != null) return _indexes;

			var indexes = new List<uint>();
			foreach (Mesh mesh in Model.Meshes)
			{
				indexes.AddRange(mesh.Elements.ToList());
			}

			_indexes = indexes.ToArray();

			return _indexes;
		}

		public int[] GetBoneIdsPerVertex()
		{
			if (_boneIdsPerVertex != null) return _boneIdsPerVertex;
			BuildPerVertexBoneData();
			return _boneIdsPerVertex;
		}

		public float[] GetBoneWeightsPerVertex()
		{
			if (_boneWeightsPerVertex != null) return _boneWeightsPerVertex;
			BuildPerVertexBoneData();
			return _boneWeightsPerVertex;
		}

		private void BuildPerVertexBoneData()
		{
			var boneIds = new List<int>();
			var weights = new List<float>();

			foreach (Mesh mesh in Model.Meshes)
			{
				// Build lookup: vertexIndex -> list of (boneIndex, weight)
				var vertexBoneMap = new Dictionary<int, List<(int boneIndex, float weight)>>();
				for (int boneIdx = 0; boneIdx < mesh.Skeleton.Bones.Count; boneIdx++)
				{
					Bone bone = mesh.Skeleton.Bones[boneIdx];
					foreach (Tuple<int, float> vw in bone.VertexWeights)
					{
						if (!vertexBoneMap.ContainsKey(vw.Item1))
							vertexBoneMap[vw.Item1] = new List<(int, float)>();
						vertexBoneMap[vw.Item1].Add((boneIdx, vw.Item2));
					}
				}

				for (int v = 0; v < mesh.Vertices.Count; v++)
				{
					var entries = vertexBoneMap.ContainsKey(v)
						? vertexBoneMap[v].OrderByDescending(e => e.weight).Take(MaxBonesPerVertex).ToList()
						: new List<(int boneIndex, float weight)>();

					// Normalize weights so they sum to 1.0
					float totalWeight = entries.Sum(e => e.weight);

					for (int i = 0; i < MaxBonesPerVertex; i++)
					{
						if (i < entries.Count)
						{
							boneIds.Add(entries[i].boneIndex);
							weights.Add(totalWeight > 0 ? entries[i].weight / totalWeight : 0f);
						}
						else
						{
							boneIds.Add(0);
							weights.Add(0f);
						}
					}
				}
			}

			_boneIdsPerVertex = boneIds.ToArray();
			_boneWeightsPerVertex = weights.ToArray();
		}

	}
}
