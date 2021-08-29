using System.Numerics;

namespace IeCoreEntities.Model
{
	/// <summary>
	/// Determines vertex value.
	/// </summary>
	public class Vertex
	{
		/// <summary>
		/// Stores position of vertex.
		/// </summary>
		public Vector3 Position = Vector3.Zero;
		/// <summary>
		/// Stores normal vector for vertex.
		/// </summary>
		public Vector3 Normal = Vector3.Zero;
		/// <summary>
		/// Stores texture coordinates for this vertex.
		/// </summary>
		public Vector2 TextureCoordinates = Vector2.Zero;

		/// <summary>
		/// Converts vertex to float array.
		/// </summary>
		/// <returns></returns>
		public float[] FloatArray()
		{
			return new[] {
					Position.X, Position.Y, Position.Z,
					TextureCoordinates.X, TextureCoordinates.Y
			};
		}



		/// <summary>
		/// <inheritdoc cref="object.ToString()"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[Vertex: P({0:D2},{1},{2}), N({3},{4},{5}), TC({6},{7})]",
								 Position.X, Position.Y, Position.Z,
								 Normal.X, Normal.Y, Normal.Z,
								 TextureCoordinates.X, TextureCoordinates.Y);
		}
	}
}
