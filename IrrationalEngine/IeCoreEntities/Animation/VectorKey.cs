using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Represents vector3 value in time
	/// </summary>
	public struct VectorKey
	{
		/// <summary>
		/// Time frame of a key.
		/// </summary>
		public double TimeFrame;
		
		/// <summary>
		/// Value of key at this moment.
		/// </summary>
		public Vector3 Value;
	}
}
