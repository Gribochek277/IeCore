using System.Collections.Generic;
using System.Numerics;

namespace IeCoreEntities.Animation
{
	/// <summary>
	/// Determines pose.
	/// </summary>
	public class BoneAnimationKeys
	{
		/// <summary>
		/// Bone id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Bone name.
		/// </summary>
		public string BoneName { get; set; }
		
		/// <summary>
		/// Contains position of bone in provided moment.
		/// </summary>
		public List<VectorKey> BonePositions { get; set; } = new List<VectorKey>();

		/// <summary>
		/// Contains scale of bone in provided moment.
		/// </summary>
		public List<VectorKey> BoneScales { get; set; } = new List<VectorKey>();

		/// <summary>
		/// Contains rotation of bone in provided moment.
		/// </summary>
		public List<QuaternionKey> BoneRotations { get; set; } = new List<QuaternionKey>();

		/// <summary>
		/// Local transform matrix
		/// </summary>
		public Matrix4x4 LocalTransform = Matrix4x4.Identity;
		
		/// <summary>
		/// Update bone state.
		/// </summary>
		/// <param name="animationTime"></param>
		public void Update(float animationTime)
		{
			Matrix4x4 translation = InterpolatePosition(animationTime);
			Matrix4x4 rotation = InterpolateRotation(animationTime);
			Matrix4x4 scale = InterpolateScaling(animationTime);
			//Matrix4x4.Invert(translation, out Matrix4x4 invertedTranslation);
			//
			//Matrix4x4.Invert(rotation, out Matrix4x4 invertedRotation);
			//
			//Matrix4x4.Invert(scale, out Matrix4x4 invertedScale);
			LocalTransform = translation * rotation * scale;
		}
		
		private int GetPositionIndex(float animationTime)
		{
			for (int index = 0; index < BonePositions.Count; index++)
			{
				if (animationTime < BonePositions[index + 1].TimeFrame)
					return index;
			}
			return 0;
		}
		
		private int GetScalingIndex(float animationTime)
		{
			for (int index = 0; index < BoneScales.Count; index++)
			{
				if (animationTime < BoneScales[index + 1].TimeFrame)
					return index;
			}
			return 0;
		}
		
		private int GetRotationIndex(float animationTime)
		{
			for (int index = 0; index < BoneRotations.Count; index++)
			{
				if (animationTime < BoneRotations[index + 1].TimeFrame)
					return index;
			}
			return 0;
		}
		
		/* Gets normalized value for Lerp & Slerp*/
		private double GetScaleFactor(double lastTimeStamp, double nextTimeStamp, float animationTime)
		{
			double scaleFactor = 0.0d;
			double midWayLength = (double)animationTime - lastTimeStamp;
			double framesDiff = nextTimeStamp - lastTimeStamp;
			scaleFactor = midWayLength / framesDiff;
			return scaleFactor;
		}
		
		private  Matrix4x4 InterpolatePosition(float animationTime)
		{
			if (1 == BonePositions.Count)
			{
				Matrix4x4 result = Matrix4x4.Identity;
				result.Translation = BonePositions[0].Value;
				return result;
			}
				

			int p0Index = GetPositionIndex(animationTime);
			int p1Index = p0Index + 1;
			double scaleFactor = GetScaleFactor(BonePositions[p0Index].TimeFrame,
				BonePositions[p1Index].TimeFrame, animationTime);
			Vector3 finalPosition = Vector3.Lerp(BonePositions[p0Index].Value,
				BonePositions[p1Index].Value, (float)scaleFactor);

			//Matrix4x4.Invert(, out Matrix4x4 res);
			return Matrix4x4.CreateTranslation(finalPosition);
		}
		
		private  Matrix4x4 InterpolateScaling(float animationTime)
		{
			if (1 == BoneScales.Count)
			{
				Matrix4x4 result = Matrix4x4.Identity;
				result.Translation = BoneScales[0].Value;
				return result;
			}
				

			int p0Index = GetScalingIndex(animationTime);
			int p1Index = p0Index + 1;
			double scaleFactor = GetScaleFactor(BoneScales[p0Index].TimeFrame,
				BoneScales[p1Index].TimeFrame, animationTime);
			Vector3 finalScale = Vector3.Lerp(BoneScales[p0Index].Value,
				BoneScales[p1Index].Value, (float)scaleFactor);
			
			
			return Matrix4x4.CreateScale(finalScale);
		}
		
		private  Matrix4x4 InterpolateRotation(float animationTime)
		{
			if (1 == BoneRotations.Count)
			{
				return Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(BoneRotations[0].Value)) ;
			}
				

			int p0Index = GetRotationIndex(animationTime);
			int p1Index = p0Index + 1;
			double scaleFactor = GetScaleFactor(BoneRotations[p0Index].TimeFrame,
				BoneRotations[p1Index].TimeFrame, animationTime);
			Quaternion finalRotation = Quaternion.Slerp(BoneRotations[p0Index].Value,
				BoneRotations[p1Index].Value, (float)scaleFactor);

			//Matrix4x4.Invert(Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(finalRotation)), out Matrix4x4 res);
			return Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(finalRotation));
		}

	}
}
