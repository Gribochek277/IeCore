using System.Collections.Generic;
using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Determines pose.
	/// </summary>
	public class AnimationKey
	{
		/// <summary>
		/// Time of animation at which this pose should be applied.
		/// </summary>
		public double TimeFrame { get; set; }
		/// <summary>
		/// Contains position of each bone in current moment.
		/// </summary>
		public Dictionary<string, Vector3> BonePositions { get; set; } = new Dictionary<string, Vector3>();

		/// <summary>
		/// Contains scale of each bone in current moment.
		/// </summary>
		public Dictionary<string, Vector3> BoneScales { get; set; } = new Dictionary<string, Vector3>();

		/// <summary>
		/// Contains rotation of each bone in current moment.
		/// </summary>
		public Dictionary<string, Quaternion> BoneRotations { get; set; } = new Dictionary<string, Quaternion>();
	}
}
