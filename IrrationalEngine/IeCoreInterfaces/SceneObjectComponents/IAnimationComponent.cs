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
    }
}
