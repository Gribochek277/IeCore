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
		/// Offset matrix (transforms from model space to bone space).
		/// </summary>
		public Matrix4x4 OffsetMatrix { get; set; }

		/// <summary>
		/// Rest-pose local transform of this bone relative to its parent.
		/// </summary>
		public Matrix4x4 LocalTransform { get; set; } = Matrix4x4.Identity;
	}
}
