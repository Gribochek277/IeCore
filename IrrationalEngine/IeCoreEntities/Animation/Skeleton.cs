using System.Collections.Generic;
using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Determines hierarchy of bones in skeleton.
	/// </summary>
	public class Skeleton
	{
		/// <summary>
		/// List of bones of this mesh.
		/// </summary>
		public List<Bone> Bones { get; set; } = new List<Bone>();

		/// <summary>
		/// Inverse of the scene root node's global transform.
		/// Required to map from world space back into the mesh's local coordinate system.
		/// </summary>
		public Matrix4x4 GlobalInverseTransform { get; set; } = Matrix4x4.Identity;
	}
}
