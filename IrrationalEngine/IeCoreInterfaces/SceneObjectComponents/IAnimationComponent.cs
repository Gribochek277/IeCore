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
		void ApplyPose(int posNum);

		/// <summary>
		/// <see cref="IModelComponent"/> Is a dependency for IAnimatioComponent
		/// </summary>
		IModelComponent ModelComponent { set; }

		/// <summary>
		/// Advances animation by the given delta time (in seconds).
		/// </summary>
		/// <param name="deltaTime"></param>
		void Update(double deltaTime);

		/// <summary>
		/// Returns the final bone transformation matrices for the current animation frame.
		/// </summary>
		/// <returns></returns>
		Matrix4x4[] GetFinalBoneMatrices();

		/// <summary>
		/// Whether this component has a valid animation to play.
		/// </summary>
		bool IsAnimated { get; }

		/// <summary>
		/// Index of the currently active animation in the model's animation array.
		/// Setting this resets the playhead to the start of the new animation.
		/// </summary>
		int AnimationIndex { get; set; }

		/// <summary>
		/// Total number of animations available on the model.
		/// </summary>
		int AnimationCount { get; }
	}
}
