using System.Collections.Generic;
using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Bone entity.
	/// </summary>
	public class Bone
	{
		/// <summary>
		/// Bone index in offset matrix
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Name of a bone.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Parent bone name.
		/// </summary>
		public string ParentName { get; set; }

		/// <summary>
		/// Contains collections of names of child bones.
		/// </summary>
		public List<string> ChildNames { get; set; }

		/// <summary>
		/// Offset matrix, animated transform
		/// </summary>
		public Matrix4x4 OffsetMatrix { get; set; }

		/// <summary>
		/// Transform matrix of node. Local space transform
		/// </summary>
		public Matrix4x4 NodeTransformMatrix { get; set; }
	}
}
