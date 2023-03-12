using System;
using System.Collections.Generic;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Determines animation asset.
	/// </summary>
	public class Animation : Asset
	{
		/// <summary>
		/// Frame rate of animation.
		/// </summary>
		public int TicksPerSecond { get; set; }

		/// <summary>
		/// Animation length in ticks.
		/// </summary>
		public double DurationInTicks { get; set; }

		/// <summary>
		/// Collection of animation keys.
		/// </summary>
		public List<BoneAnimationKeys> Keys { get; set; } = new List<BoneAnimationKeys>();

		/// <summary>
		/// <inheritdoc cref="Asset"/>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="file"></param>
		public Animation(string name, string file) : base(name, file)
		{
		}

		/// <summary>
		/// <inheritdoc cref="Asset"/>
		/// </summary>
		//TODO: Make animation properly registered asset
		public Animation() : base(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
		{
		}
		
	}
}
