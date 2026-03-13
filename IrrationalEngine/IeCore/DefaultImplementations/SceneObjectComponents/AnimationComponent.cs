using IeCoreEntities.Animation;
using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class AnimationComponent : IAnimationComponent
	{
		public IModelComponent ModelComponent { private get; set; }
		private Model _model;
		private double _currentTime;
		private Matrix4x4[] _finalBoneMatrices;
		/// <summary>Set of bone names that have at least one keyframe in the animation.</summary>
		private HashSet<string> _animatedBoneNames;

		public string Name => "AnimationSceneObjectComponent";

		private int _animationIndex;
		public int AnimationIndex
		{
			get => _animationIndex;
			set
			{
				if (_model == null || value < 0 || value >= _model.Animations.Count) return;
				_animationIndex = value;
				_currentTime = 0;
				RebuildAnimatedBoneNames();
			}
		}

		public int AnimationCount => _model?.Animations.Count ?? 0;

		public bool IsAnimated =>
			_model != null
			&& _model.Animations.Count > 0
			&& _model.Meshes.Count > 0
			&& _model.Meshes[0].Skeleton.Bones.Count > 0;

		public void ApplyPose(int posNum)
		{
			if (!IsAnimated) return;
			Animation animation = _model.Animations[_animationIndex];
			if (posNum < animation.Keys.Count)
				_currentTime = animation.Keys[posNum].TimeFrame;
			ComputeBoneMatrices(animation);
		}

		public void Update(double deltaTime)
		{
			if (!IsAnimated) return;

			Animation animation = _model.Animations[_animationIndex];
			// Guard against TicksPerSecond being 0 (some FBX exports omit it)
			double effectiveRate = animation.FrameRate > 0 ? animation.FrameRate : 25.0;
			_currentTime += deltaTime * effectiveRate;

			if (animation.Ticks > 0)
				_currentTime %= animation.Ticks;

			ComputeBoneMatrices(animation);
		}

		public Matrix4x4[] GetFinalBoneMatrices() => _finalBoneMatrices;

		private void ComputeBoneMatrices(Animation animation)
		{
			Mesh mesh = _model.Meshes[0];
			Skeleton skeleton = mesh.Skeleton;
			int boneCount = skeleton.Bones.Count;

			if (_finalBoneMatrices == null || _finalBoneMatrices.Length != boneCount)
				_finalBoneMatrices = new Matrix4x4[boneCount];

			var boneIndexMap = new Dictionary<string, int>(boneCount);
			for (int i = 0; i < boneCount; i++)
				boneIndexMap[skeleton.Bones[i].Name] = i;

			int[] order = TopologicalSortBones(skeleton, boneIndexMap);
			var globalTransforms = new Matrix4x4[boneCount];

			for (int o = 0; o < boneCount; o++)
			{
				int i = order[o];
				Bone bone = skeleton.Bones[i];

				Matrix4x4 localTransform;
				if (_animatedBoneNames.Contains(bone.Name))
				{
					// Per-bone, per-component sampling: each component finds its own
					// surrounding keys so sparse keyframe channels don't default to zero.
					Vector3 position = SampleVector3(animation.Keys, _currentTime, bone.Name,
						k => k.BonePositions, Vector3.Zero);
					Quaternion rotation = SampleQuaternion(animation.Keys, _currentTime, bone.Name);
					Vector3 scale = SampleVector3(animation.Keys, _currentTime, bone.Name,
						k => k.BoneScales, Vector3.One);

					localTransform = Matrix4x4.CreateScale(scale)
						* Matrix4x4.CreateFromQuaternion(rotation)
						* Matrix4x4.CreateTranslation(position);
				}
				else
				{
					// Bone has no animation data — use its rest-pose local transform.
					localTransform = bone.LocalTransform;
				}

				if (string.IsNullOrEmpty(bone.ParentName) || !boneIndexMap.ContainsKey(bone.ParentName))
					globalTransforms[i] = localTransform;
				else
					globalTransforms[i] = localTransform * globalTransforms[boneIndexMap[bone.ParentName]];

				// Standard Assimp skinning formula:
				// finalMatrix = OffsetMatrix * GlobalBoneTransform * GlobalInverseTransform
				// (row-major: offset first, then global, then root correction)
				_finalBoneMatrices[i] = bone.OffsetMatrix * globalTransforms[i] * skeleton.GlobalInverseTransform;
			}
		}

		/// <summary>
		/// Samples a Vector3 component for a specific bone at the given time.
		/// Searches for the nearest keys that actually contain data for this bone,
		/// so sparse channels don't collapse to zero.
		/// </summary>
		private static Vector3 SampleVector3(
			IList<AnimationKey> keys, double time, string boneName,
			System.Func<AnimationKey, Dictionary<string, Vector3>> selector,
			Vector3 defaultValue)
		{
			AnimationKey prevKey = null, nextKey = null;

			for (int i = 0; i < keys.Count; i++)
			{
				if (!selector(keys[i]).ContainsKey(boneName)) continue;

				if (keys[i].TimeFrame <= time)
					prevKey = keys[i];
				else if (nextKey == null)
				{
					nextKey = keys[i];
					break;
				}
			}

			if (prevKey == null) return nextKey != null ? selector(nextKey)[boneName] : defaultValue;
			if (nextKey == null) return selector(prevKey)[boneName];

			double span = nextKey.TimeFrame - prevKey.TimeFrame;
			float t = span > 0 ? (float)((time - prevKey.TimeFrame) / span) : 0f;
			return Vector3.Lerp(selector(prevKey)[boneName], selector(nextKey)[boneName], t);
		}

		/// <summary>
		/// Samples a Quaternion rotation for a specific bone at the given time.
		/// </summary>
		private static Quaternion SampleQuaternion(IList<AnimationKey> keys, double time, string boneName)
		{
			AnimationKey prevKey = null, nextKey = null;

			for (int i = 0; i < keys.Count; i++)
			{
				if (!keys[i].BoneRotations.ContainsKey(boneName)) continue;

				if (keys[i].TimeFrame <= time)
					prevKey = keys[i];
				else if (nextKey == null)
				{
					nextKey = keys[i];
					break;
				}
			}

			if (prevKey == null) return nextKey != null ? nextKey.BoneRotations[boneName] : Quaternion.Identity;
			if (nextKey == null) return prevKey.BoneRotations[boneName];

			double span = nextKey.TimeFrame - prevKey.TimeFrame;
			float t = span > 0 ? (float)((time - prevKey.TimeFrame) / span) : 0f;
			return Quaternion.Slerp(prevKey.BoneRotations[boneName], nextKey.BoneRotations[boneName], t);
		}

		private static int[] TopologicalSortBones(Skeleton skeleton, Dictionary<string, int> boneIndexMap)
		{
			int count = skeleton.Bones.Count;
			var sorted = new List<int>(count);
			var visited = new bool[count];

			void Visit(int idx)
			{
				if (visited[idx]) return;
				Bone bone = skeleton.Bones[idx];
				if (!string.IsNullOrEmpty(bone.ParentName) && boneIndexMap.TryGetValue(bone.ParentName, out int parentIdx))
					Visit(parentIdx);
				visited[idx] = true;
				sorted.Add(idx);
			}

			for (int i = 0; i < count; i++)
				Visit(i);

			return sorted.ToArray();
		}

		public void OnLoad()
		{
			_model = ModelComponent.Model;
			// Sort each animation's keys by TimeFrame — the FBX importer may produce
			// them in arbitrary order (Distinct() doesn't sort), which breaks sampling.
			foreach (Animation animation in _model.Animations)
				animation.Keys = animation.Keys.OrderBy(k => k.TimeFrame).ToList();
			_currentTime = 0;
			_animationIndex = 0;
			RebuildAnimatedBoneNames();

			if (_model.Meshes.Count > 0 && _model.Meshes[0].Skeleton.Bones.Count > 0)
			{
				int boneCount = _model.Meshes[0].Skeleton.Bones.Count;
				_finalBoneMatrices = new Matrix4x4[boneCount];
				for (int i = 0; i < boneCount; i++)
					_finalBoneMatrices[i] = Matrix4x4.Identity;
			}

			if (IsAnimated)
				ApplyPose(0);
		}

		public void OnUnload()
		{
			_finalBoneMatrices = null;
			_currentTime = 0;
		}

		private void RebuildAnimatedBoneNames()
		{
			_animatedBoneNames = new HashSet<string>();
			if (_model.Animations.Count > 0 && _animationIndex < _model.Animations.Count)
			{
				foreach (AnimationKey key in _model.Animations[_animationIndex].Keys)
				{
					foreach (string name in key.BonePositions.Keys) _animatedBoneNames.Add(name);
					foreach (string name in key.BoneRotations.Keys) _animatedBoneNames.Add(name);
					foreach (string name in key.BoneScales.Keys) _animatedBoneNames.Add(name);
				}
			}
		}
	}
}
