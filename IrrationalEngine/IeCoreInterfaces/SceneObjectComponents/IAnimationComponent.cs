using System.Numerics;

namespace IeCoreInterfaces.SceneObjectComponents
{
	/// <summary>
	/// Represents scene object component which responsible for animation.
	/// </summary>
	public interface IAnimationComponent : ISceneObjectComponent
	{
		/// <summary>
		/// Apply pose number
		/// </summary>
		/// <param name="posNum"></param>
		public void ApplyPose(int posNum);

		/// <summary>
		/// <see cref="IModelComponent"/> Is a dependency for IAnimatioComponent
		/// </summary>
		public IModelComponent ModelComponent { set; }

		/// <summary>
		/// Update animation
		/// </summary>
		/// <param name="deltaTime"></param>
		void UpdateAnimation(float deltaTime);

		/// <summary>
		/// Returns final bone matrices
		/// </summary>
		Matrix4x4[] FinalBonesMatrices { get; }
	}
}
