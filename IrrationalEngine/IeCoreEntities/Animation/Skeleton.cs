using System.Collections.Generic;

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
	}
}
