using Assimp;
using AutoMapper;
using IeCore.Extensions;
using IeCoreEntities;
using IeCoreEntities.Animation;
using IeCoreEntities.Extensions;
using IeCoreEntities.Model;
using IeCoreInterfaces.AssetImporters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Animation = Assimp.Animation;
using Bone = Assimp.Bone;
using Mesh = Assimp.Mesh;
using QuaternionKey = Assimp.QuaternionKey;
using VectorKey = Assimp.VectorKey;

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
			using (AssimpContext context = new AssimpContext())
			{
				const PostProcessSteps assimpLoadFlags = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
				Scene loadedAssimpScene = context.ImportFile(file, assimpLoadFlags);
				if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
				{
					Console.WriteLine("Scene error");
				}

				Model model = new Model(Path.GetFileName(file), file);

				foreach (Mesh assimpMesh in loadedAssimpScene.Meshes)
					model.Meshes.Add(
						_mapper.Map<Mesh, IeCoreEntities.Model.Mesh>(assimpMesh, opt =>
							{
								opt.AfterMap((mappedAssimpMesh, dest) =>
								{
									//Initialize default values for bones;
									//TODO: Consider extraction or another approach.
									foreach (Vertex vertex in dest.Vertices)
									{
										for (int i = 0; i < Vertex.MaxBones; i++)
										{
											vertex.Weights[i] = 0.0f;
											vertex.BoneIDs[i] = -1;
										}
									}
									if (!mappedAssimpMesh.HasBones) return;
									MapBones(mappedAssimpMesh, dest, loadedAssimpScene);
									
									dest.Skeleton.DrawInConsole();
								});
							})
						);
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
				
				//TODO: Possible animation with several meshes, should be linked. Investigate.
				return model;
			}
		}

		private void MapBones(Mesh assimpMesh, IeCoreEntities.Model.Mesh destination, Scene loadedAssimpScene)
		{
			for (int i = 0; i<  assimpMesh.Bones.Count(); i++)
			{
				
				destination.Skeleton.Bones.Add(_mapper.Map<Bone, IeCoreEntities.Animation.Bone>(assimpMesh.Bones[i], opt =>
				{
					opt.AfterMap((_, dest) =>
					{
						Node node = SearchNodeByName(loadedAssimpScene.RootNode, dest.Name);

						dest.Id = i;
						dest.NodeTransformMatrix =  node.Transform.ToNumericMatrix();
						dest.ChildNames = node.Children.Select(x => x.Name).ToList();

						foreach (VertexWeight vertexWeight in assimpMesh.Bones[i].VertexWeights)
						{
							Vertex vertex = destination.Vertices[vertexWeight.VertexID];
							for (int j = 0; j < Vertex.MaxBones; j++)
							{
								if(vertex.BoneIDs[j] == -1) {
									vertex.Weights[j] = vertexWeight.Weight;
									vertex.BoneIDs[j] = dest.Id;
									break;
								}
							}
						}
						
						if (node.Parent != null)
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
		private static List<BoneAnimationKeys> NodeAnimationChannelToPose(IReadOnlyCollection<NodeAnimationChannel> nodeAnimationChannels)
		{
			List<BoneAnimationKeys> resultPoses = new List<BoneAnimationKeys>();


			//Enrich poses with data
			foreach (NodeAnimationChannel nodeAnimationChannel in nodeAnimationChannels)
			{
				BoneAnimationKeys boneAnimationKeys = new BoneAnimationKeys();
				boneAnimationKeys.BoneName = nodeAnimationChannel.NodeName;
				if (nodeAnimationChannel.HasPositionKeys)
				{
					boneAnimationKeys.BonePositions = nodeAnimationChannel.PositionKeys.Select(x =>
						new IeCoreEntities.Animation.VectorKey()
						{
							TimeFrame = x.Time,
							Value = x.Value.ToVector3()
						}).ToList();
				}
				
				if (nodeAnimationChannel.HasScalingKeys)
				{
					boneAnimationKeys.BoneScales = nodeAnimationChannel.ScalingKeys.Select(x =>
						new IeCoreEntities.Animation.VectorKey()
						{
							TimeFrame = x.Time,
							Value = x.Value.ToVector3()
						}).ToList();
				}
				
				if (nodeAnimationChannel.HasPositionKeys)
				{
					boneAnimationKeys.BoneRotations = nodeAnimationChannel.RotationKeys.Select(x =>
						new IeCoreEntities.Animation.QuaternionKey()
						{
							TimeFrame = x.Time,
							Value = x.Value.ToQuaternion()
						}).ToList();
				}
				
				resultPoses.Add(boneAnimationKeys);
			}

			return resultPoses;
		}
	}
}
