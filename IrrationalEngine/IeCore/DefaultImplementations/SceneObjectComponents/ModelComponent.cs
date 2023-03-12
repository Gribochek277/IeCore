using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;
using System.Collections.Generic;
using System.Linq;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class ModelComponent : IModelComponent
	{

		public string Name => "ModelSceneObjectComponent";

		public Model Model { get; private set; }

		private uint[] _indexes;
		private float[] _vboTextureData;
		private float[] _vboPositionData;
		private float[] _vboWeightData;
		private int[] _vboBoneIdsData;

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

		public float[] GetVboPositionDataOfModel()
		{
			if (_vboPositionData != null) return _vboPositionData;
			List<float> positionData = new List<float>();
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

		public float[] GetVboTextureDataOfModel()
		{
			if (_vboTextureData != null) return _vboTextureData;

			List<float> textureData = new List<float>();
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
		
		public float[] GetVboWeightsDataOfModel()
		{
			if (_vboWeightData != null) return _vboWeightData;

			List<float> weightData = new List<float>();
			foreach (Mesh mesh in Model.Meshes)
			{
				foreach (Vertex vertex in mesh.Vertices)
				{
					weightData.Add(vertex.Weights[0]);
					weightData.Add(vertex.Weights[1]);
					weightData.Add(vertex.Weights[2]);
					weightData.Add(vertex.Weights[3]);
				}
			}

			_vboWeightData = weightData.ToArray();

			return _vboWeightData;
		}
		
		public int[] GetVboBoneIdsDataOfModel()
		{
			if (_vboBoneIdsData != null) return _vboBoneIdsData;

			List<int> boneIdData = new List<int>();
			foreach (Mesh mesh in Model.Meshes)
			{
				foreach (Vertex vertex in mesh.Vertices)
				{
					boneIdData.Add(vertex.BoneIDs[0]);
					boneIdData.Add(vertex.BoneIDs[1]);
					boneIdData.Add(vertex.BoneIDs[2]);
					boneIdData.Add(vertex.BoneIDs[3]);
				}
			}

			_vboBoneIdsData = boneIdData.ToArray();

			return _vboBoneIdsData;
		}

		public uint[] GetIndexesOfModel() //TODO: Add caching;
		{
			if (_indexes != null) return _indexes;

			List<uint> indexes = new List<uint>();
			foreach (Mesh mesh in Model.Meshes)
			{
				indexes.AddRange(mesh.Elements.ToList());
			}

			_indexes = indexes.ToArray();

			return _indexes;
		}

	}
}
