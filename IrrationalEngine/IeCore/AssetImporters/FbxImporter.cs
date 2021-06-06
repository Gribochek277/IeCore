using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assimp;
using AutoMapper;
using IeCore.Extensions;
using IeCoreEntities;
using IeCoreEntities.Animation;
using IeCoreEntities.Extensions;
using IeCoreEntities.Model;
using IeCoreInterfaces.AssetImporters;
using Animation = Assimp.Animation;
using Bone = Assimp.Bone;
using Mesh = Assimp.Mesh;

namespace IeCore.AssetImporters
{
    public class FbxImporter : IFbxImporter
    {
        private readonly IMapper _mapper;
        public Type AssetType => typeof(Model);

        public string[] FileExtensions => new[] { ".fbx" };

        public FbxImporter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Asset Import(string file)
        {
            using (var context = new AssimpContext()) 
            { 
                const PostProcessSteps assimpLoadFlags = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
                Scene loadedAssimpScene = context.ImportFile(file, assimpLoadFlags);
                if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
                {
                    Console.WriteLine("Scene error");
                }

                var model = new Model(Path.GetFileName(file), file);
                if (loadedAssimpScene.HasAnimations)
                {
                    foreach (Animation assimpAnimation in loadedAssimpScene.Animations)
                    {
                        model.Animations.Add(_mapper.Map<Animation, IeCoreEntities.Animation.Animation>(assimpAnimation, opt =>
                        {
                            opt.AfterMap((mappedAssimpAnimation, dest) =>
                                {
                                    if (!mappedAssimpAnimation.HasNodeAnimations) return;
                                    dest.Keys = NodeAnimationChannelToPose(mappedAssimpAnimation.NodeAnimationChannels);
                                }
                            );
                        }));
                    }
                }
                foreach(Mesh assimpMesh in loadedAssimpScene.Meshes)
                    model.Meshes.Add(
                        _mapper.Map<Mesh, IeCoreEntities.Model.Mesh>(assimpMesh, opt =>
                            {
                                opt.AfterMap((mappedAssimpMesh, dest) =>
                                {
                                    if (!mappedAssimpMesh.HasBones) return;
                                    MapBones(mappedAssimpMesh, dest, loadedAssimpScene);                                        
                                    dest.Skeleton.DrawInConsole();
                                });
                            })
                        );
                //TODO: Possible animation with several meshes, should be linked. Investigate.
                return model;
            }
        }

        private void MapBones(Mesh assimpMesh, IeCoreEntities.Model.Mesh destination, Scene loadedAssimpScene)
        {
            foreach (Bone bone in assimpMesh.Bones)
            {
                destination.Skeleton.Bones.Add(_mapper.Map<Bone, IeCoreEntities.Animation.Bone>(bone, opt =>
                {
                    opt.AfterMap((_, dest) =>
                    {
                      var node = SearchNodeByName(loadedAssimpScene.RootNode, dest.Name);

                    if(node.Parent != null)
                      dest.ParentName = node.Parent.Name;
                    });
                   
                }));
            }

            //Find and rename root node.
            foreach (IeCoreEntities.Animation.Bone bone in destination.Skeleton.Bones)
            {
                if (destination.Skeleton.Bones.Find(x => x.Name == bone.ParentName) == null)
                    bone.ParentName = string.Empty;
            }

        }

        private static Node SearchNodeByName(Node node, string nodeName)
        {
            if (!node.HasChildren)
                return null;
            if (node.Name == nodeName) 
                return node;
            foreach (Node child in node.Children)
            {
                Node result = SearchNodeByName(child, nodeName);
                if (result != null)
                    return result;
            }
            return null;
        }
        private static List<AnimationKey> NodeAnimationChannelToPose(IReadOnlyCollection<NodeAnimationChannel> nodeAnimationChannels)
        {
            var resultPoses = new List<AnimationKey>();

            var uniqueTimeFrames = nodeAnimationChannels.Select(nodeAnimationChannel =>nodeAnimationChannel.PositionKeys.Select(key => key.Time)
                    .Union(nodeAnimationChannel.ScalingKeys.Select(vectorKey => vectorKey.Time))
                    .Union(nodeAnimationChannel.RotationKeys.Select(quaternionKey => quaternionKey.Time))).SelectMany(x=>x).Distinct().ToList();

            //Prepare list of poses
            foreach (double timeFrame in uniqueTimeFrames)
            {
                var pose = new AnimationKey
                {
                    TimeFrame = timeFrame
                };
                resultPoses.Add(pose);
            }

            //Enrich poses with data
            foreach (NodeAnimationChannel nodeAnimationChannel in nodeAnimationChannels)
            {
                foreach (VectorKey key in nodeAnimationChannel.PositionKeys)
                {
                    //TODO: Fix floating comparation
                    resultPoses.First(x => x.TimeFrame == key.Time).BonePositions.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToVector3());
                }

                foreach (VectorKey key in nodeAnimationChannel.ScalingKeys)
                {
                    //TODO: Fix floating comparation
                    resultPoses.First(x => x.TimeFrame == key.Time).BoneScales.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToVector3());
                }

                foreach (QuaternionKey key in nodeAnimationChannel.RotationKeys)
                {
                    //TODO: Fix floating comparation
                    resultPoses.First(x => x.TimeFrame == key.Time).BoneRotations.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToQuaternion());
                }
            }

            return resultPoses;
        }
    }
}
