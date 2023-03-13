using IeCoreEntities.Animation;
using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;
using System;
using System.Linq;
using System.Numerics;
using IeCoreEntities.Extensions;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class AnimationComponent : IAnimationComponent
	{
		private float  _currentTime = 0f;
		private Animation _currentAnimation;
		private Matrix4x4[] _finalBonesMatrices = new Matrix4x4[100];
		private float _deltaTime = 0f;
		private Model _model;
		public Matrix4x4[] FinalBonesMatrices => _finalBonesMatrices;
		
		public void ApplyPose(int posNum)
		{
			//throw new NotImplementedException();
		}

		public IModelComponent ModelComponent { private get; set; }
		public string Name => "AnimationSceneObjectComponent";
		
		public void UpdateAnimation(float deltaTime)
		{
			_deltaTime = deltaTime;
			if (_currentAnimation != null)
			{
				_currentTime += _currentAnimation.TicksPerSecond * _deltaTime;
				_currentTime = _currentTime % (float)_currentAnimation.DurationInTicks;
				CalculateBoneTransform(_model.Meshes.FirstOrDefault().Skeleton.GetRootBoneInfo(), Matrix4x4.Identity);
			}
		}

		private void CalculateBoneTransform(Bone bone, Matrix4x4 parentTransform)
		{
			if(bone == null)
				return;

			
			Matrix4x4 transform = bone.OffsetMatrix;
			BoneAnimationKeys boneKey = _currentAnimation.Keys.FirstOrDefault(x => x.BoneName == bone.Name);

			if (boneKey != null)
			{
				boneKey.Update(_currentTime);
				transform = boneKey.LocalTransform;
			}

		//	Matrix4x4.Invert(transform, out  Matrix4x4 invertedTransform);
		    //Matrix4x4 s = Matrix4x4.Multiply(transform, bone.OffsetMatrix);
			Matrix4x4 globalTransformation = Matrix4x4.Multiply(parentTransform, transform);
			//Matrix4x4.Invert(globalTransformation, out Matrix4x4 invertedGlobal);

			int index = bone.Id;

			//Matrix4x4.Invert(bone.OffsetMatrix, out var offsetMatrix);


			_finalBonesMatrices[index] = globalTransformation;//Matrix4x4.Multiply(globalTransformation,bone.OffsetMatrix);
			
			foreach (string boneInfoChildName in bone.ChildNames)
			{
				CalculateBoneTransform(_model.Meshes[0].Skeleton.Bones.FirstOrDefault(x=>x.Name == boneInfoChildName), globalTransformation);
			}
		}


		public void OnLoad()
		{
			//throw new System.NotImplementedException();
			_model = ModelComponent.Model;
			_currentAnimation = _model.Animations.FirstOrDefault();
			for (int i = 0; i < _finalBonesMatrices.Length; i++)
			{
				_finalBonesMatrices[i] = Matrix4x4.Identity;
			}

			//UpdateAnimation(0);
		}

		public void OnUnload()
		{
			throw new NotImplementedException();
		}
	}
}
