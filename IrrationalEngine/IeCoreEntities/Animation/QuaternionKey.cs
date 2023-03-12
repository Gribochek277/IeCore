using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Represents Quaternion value in time
	/// </summary>
	public struct QuaternionKey
	{
		/// <summary>
		/// Time frame of a key.
		/// </summary>
		public double TimeFrame;
		
		/// <summary>
		/// Value of key at this moment.
		/// </summary>
		public Quaternion Value;
	}
}
