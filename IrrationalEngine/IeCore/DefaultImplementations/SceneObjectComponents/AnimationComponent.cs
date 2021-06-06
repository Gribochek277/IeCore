using System;
using System.Linq;
using IeCoreEntities.Animation;
using IeCoreEntities.Model;
using IeCoreInterfaces.SceneObjectComponents;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
    public class AnimationComponent: IAnimationComponent
    {
        public IModelComponent ModelComponent { private get;  set; }
        private Model _model;

        public string Name => "AnimationSceneObjectComponent";

        public void ApplyPose(int posNum)
        {
            Animation animation = _model.Animations[0];
            AnimationKey animationKey = animation.Keys[posNum];

            Mesh mesh = _model.Meshes[0];
            Bone rootBone = mesh.Skeleton.Bones.Find(x => x.ParentName == string.Empty);
            ApplyMatrices(rootBone, mesh.Skeleton, animationKey);
        }

        private static void ApplyMatrices(Bone bone, Skeleton skeleton, AnimationKey animationKey)
        {
            var childrenBones = skeleton.Bones.Where(x => x.ParentName == bone.Name).ToList();
            Console.WriteLine(bone.Name);
            for (var i = 0; i < childrenBones.Count; i++)
            { 
                ApplyMatrices(childrenBones[i], skeleton, animationKey);
                animationKey.BonePositions[bone.Name] = animationKey.BonePositions[bone.Name] * animationKey.BonePositions[childrenBones[i].Name];

                animationKey.BoneRotations[bone.Name] = animationKey.BoneRotations[bone.Name] * animationKey.BoneRotations[childrenBones[i].Name];

                animationKey.BoneScales[bone.Name] = animationKey.BoneScales[bone.Name] * animationKey.BoneScales[childrenBones[i].Name];
            }
        }

        public void OnLoad()
        {
            //throw new System.NotImplementedException();
            _model = ModelComponent.Model;
            ApplyPose(0);
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
