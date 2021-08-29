using System;
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
		/// Name of a bone.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Parent bone name.
		/// </summary>
		public string ParentName { get; set; }

		/// <summary>
		/// List of vertexes to which this 
		/// bone belongs and with which weight. 
		/// Int32 element is a vertex ID.
		/// Float element is weight.
		/// </summary>
		public List<Tuple<int, float>> VertexWeights { get; set; } = new List<Tuple<int, float>>();

		/// <summary>
		/// Offset matrix
		/// </summary>
		public Matrix4x4 OffsetMatrix { get; set; }
	}
}
