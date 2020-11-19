using Assimp;
using AutoMapper;
using IeCoreEntites.Model;
using IeCoreInterfaces.AssetImporters;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using IeCore.Extentions;

namespace IeCore.AssetImporters
{
    public class FbxImporter : IAssetImporter
    {
        private IMapper _mapper;
        public Type AssetType => typeof(Model);

        public string[] FileExtensions => new string[] { ".fbx" };

        public FbxImporter()
        {
            _mapper = IrrationalEngine.ServiceProvider.GetService<IMapper>();
        }

        public void Import(string file)
        {
            using (AssimpContext context = new AssimpContext()) 
            { 
                const PostProcessSteps ASSIMP_LOAD_FLAGS = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
                Scene loadedAssimpScene = context.ImportFile(file, ASSIMP_LOAD_FLAGS);
                if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
                {
                    Console.WriteLine("Scene error");
                }

                Model model = new Model(Path.GetFileName(file), file);
                if (loadedAssimpScene.HasAnimations)
                {
                    foreach (Assimp.Animation assimpAnimation in loadedAssimpScene.Animations)
                    {
                        model.Animations.Add(_mapper.Map<Assimp.Animation, IeCoreEntites.Model.Animation>(assimpAnimation, opt =>
                        {
                            opt.AfterMap((assimpAnimation, dest) =>
                            {
                                if (assimpAnimation.HasNodeAnimations)
                                {
                                    dest.Keys = NodeAnimationChannelToPose(assimpAnimation.NodeAnimationChannels);
                                }
                            }
                            );
                        }));
                    }
                }
                foreach(Assimp.Mesh assimpMesh in loadedAssimpScene.Meshes)
                    model.Meshes.Add(
                        _mapper.Map<Assimp.Mesh, IeCoreEntites.Model.Mesh>(assimpMesh, opt =>
                            {
                                opt.AfterMap((assimpMesh, dest) =>
                                {
                                    if (assimpMesh.HasBones)
                                    {
                                        foreach (Assimp.Bone bone in assimpMesh.Bones)
                                        {
                                            dest.Bones.Add(_mapper.Map<Assimp.Bone, IeCoreEntites.Model.Bone>(bone));
                                        }
                                    }
                                });
                            })
                        );
                Context.Assetmanager.Register(model);
            }
        }


        private List<Pose> NodeAnimationChannelToPose(List<NodeAnimationChannel> nodeAnimationChannels)
        {
            List<Pose> resultPoses = new List<Pose>();

            List<double> uniqueTimeFrames = nodeAnimationChannels.Select(x =>x.PositionKeys.Select(x => x.Time)
                    .Union(x.ScalingKeys.Select(x => x.Time))
                    .Union(x.RotationKeys.Select(x => x.Time))).SelectMany(x=>x).Distinct().ToList();

            //Prepare list of poses
            foreach (double timeFrame in uniqueTimeFrames)
            {
                Console.WriteLine(timeFrame);
                Pose pose = new Pose();
                pose.TimeFrame = timeFrame;
                resultPoses.Add(pose);
            }

            //Enrich poses with data
            foreach (NodeAnimationChannel nodeAnimationChannel in nodeAnimationChannels)
            {
                Console.WriteLine(nodeAnimationChannel.NodeName);
                foreach (VectorKey key in nodeAnimationChannel.PositionKeys)
                {
                    resultPoses.Where(x => x.TimeFrame == key.Time).First().BonePositions.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToVector3());
                }

                foreach (VectorKey key in nodeAnimationChannel.ScalingKeys)
                {
                    resultPoses.Where(x => x.TimeFrame == key.Time).First().BoneScales.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToVector3());
                }

                foreach (QuaternionKey key in nodeAnimationChannel.RotationKeys)
                {
                    resultPoses.Where(x => x.TimeFrame == key.Time).First().BoneRotations.TryAdd(nodeAnimationChannel.NodeName, key.Value.ToQuaternion());
                }
            }

            return resultPoses;
        }
    }
}
