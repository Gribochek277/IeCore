using IeCoreEntities.Model;

namespace IeCoreInterfaces.SceneObjectComponents
{
	/// <summary>
	/// Represents scene object component which responsible for models.
	/// </summary>
	public interface IModelComponent : ISceneObjectComponent
	{
		/// <summary>
		/// Get model which stored in this component.
		/// </summary>
		Model Model { get; }

		/// <summary>
		/// Return Position Vertex Buffer Object data from model.
		/// </summary>
		/// <returns></returns>
		float[] GetVboPositionDataOfModel();

		/// <summary>
		/// Return Texture Vertex Buffer Object data from model.
		/// </summary>
		/// <returns></returns>
		float[] GetVboTextureDataOfModel();


		/// <summary>
		/// Return Indices data from model.
		/// </summary>
		/// <returns></returns>
		uint[] GetIndexesOfModel();

		/// <summary>
		/// Returns weight for bone in each vertex.
		/// </summary>
		/// <returns></returns>
		public float[] GetVboWeightsDataOfModel();

		/// <summary>
		/// Returns bone id's which belong to each vertex.
		/// </summary>
		/// <returns></returns>
		public int[] GetVboBoneIdsDataOfModel();
	}
}
