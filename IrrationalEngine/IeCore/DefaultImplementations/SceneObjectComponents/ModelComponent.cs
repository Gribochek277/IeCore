using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class ModelComponent : IModelComponent
	{

		public string Name => "ModelSceneObjectComponent";

		public Model Model { get; private set; }

		private uint[] _indexes;
		private float[] _vboTextureData;
		private float[] _vboPositionData;

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

	}
}
