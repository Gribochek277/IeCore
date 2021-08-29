namespace IeCoreEntities.Materials
{
	/// <summary>
	/// Texture wrapping modes.
	/// </summary>
	public enum TextureWrapping
	{
		/// <summary>
		/// The default behavior for textures. Repeats the texture image.
		/// </summary>
		Repeat,
		/// <summary>
		/// Same as Repeat but mirrors the image with each repeat.
		/// </summary>
		MirrorRepeat,
		/// <summary>
		/// Clamps the coordinates between 0 and 1. 
		/// The result is that higher coordinates become clamped to the edge, resulting in a stretched edge pattern.
		/// </summary>
		ClampToEdge,
		/// <summary>
		/// Coordinates outside the range are now given a user-specified border color.
		/// </summary>
		ClampToBorder
	}
}
