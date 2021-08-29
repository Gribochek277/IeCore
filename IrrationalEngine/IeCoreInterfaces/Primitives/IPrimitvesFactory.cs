namespace IeCoreInterfaces.Primitives
{
	/// <summary>
	/// Creates primitive SceneObjects such as Cube, Box etc.
	/// </summary>
	public interface IPrimitvesFactory
	{
		/// <summary>
		/// Creates cube instance. 
		/// </summary>
		/// <returns></returns>
		public ISceneObject CreateCube();
		/// <summary>
		/// Creates rectangle instance.
		/// </summary>
		/// <returns></returns>
		public ISceneObject CreateRectangle();
		/// <summary>
		/// Creates triangle instance.
		/// </summary>
		/// <returns></returns>
		public ISceneObject CreateTriangle();
	}
}
