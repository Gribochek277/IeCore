using Assimp;
using AutoMapper;
using IeCoreEntities;
using IeCoreEntities.Animation;
using IeCoreEntities.Extensions;
using IeCoreEntities.Model;
using IeWin.Extensions;
using Animation = Assimp.Animation;
using Bone = Assimp.Bone;
using Mesh = Assimp.Mesh;

namespace IeWin.AssetImporters
{
	/// <summary>
	/// Importer for FBX model which depends on Assimp library.
	/// </summary>
	public class FbxImporter : IFbxImporter
	{
		private readonly IMapper _mapper;

		/// <inheritdoc />
		public Type AssetType => typeof(Model);

		/// <inheritdoc />
		public string[] FileExtensions => new[] { ".fbx" };

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="mapper"></param>
		public FbxImporter(IMapper mapper)
		{
			_mapper = mapper;
		}

		/// <inheritdoc />
		public Asset Import(string file)
		{
			using (AssimpContext context = new AssimpContext())
			{
				const PostProcessSteps assimpLoadFlags = PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs;
				
				Scene loadedAssimpScene = context.ImportFile(Path.Combine(Environment.CurrentDirectory,file), assimpLoadFlags);
				if (loadedAssimpScene == null || loadedAssimpScene.SceneFlags == SceneFlags.Incomplete)
				{
					Console.WriteLine("Scene error");
				}

				var model = new Model(Path.GetFileName(file), file);
				if (loadedAssimpScene != null && loadedAssimpScene.HasAnimations)
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

				if (loadedAssimpScene != null)
					foreach (Mesh assimpMesh in loadedAssimpScene.Meshes)
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
			for (int i = 0; i<  assimpMesh.Bones.Count(); i++)
			{
				
				destination.Skeleton.Bones.Add(_mapper.Map<Bone, IeCoreEntities.Animation.Bone>(assimpMesh.Bones[i], opt =>
				{
					opt.AfterMap((_, dest) =>
					{
						Node? node = SearchNodeByName(loadedAssimpScene.RootNode, dest.Name);

						if (node != null && node.Parent != null)
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

		private static Node? SearchNodeByName(Node node, string nodeName)
		{
			if (!node.HasChildren)
				return null;
			if (node.Name == nodeName)
				return node;
			foreach (Node? child in node.Children)
			{
				Node? result = SearchNodeByName(child, nodeName);
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
