using IeCoreEntities.Animation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IeCoreEntities.Extensions
{
	/// <summary>
	/// Extensions for <see cref="IeCoreEntities.Animation.Skeleton"/> class
	/// </summary>
	public static class Skeleton
	{
		/// <summary>
		/// Draw in console skeleton bone hierarchy.
		/// </summary>
		/// <param name="skeleton"></param>
		public static void DrawInConsole(this IeCoreEntities.Animation.Skeleton skeleton)
		{
			Bone rootBone = skeleton.Bones.Find(x => x.ParentName == string.Empty);
			Console.WriteLine("Skeleton hierarchy: ");
			PrintPretty("", rootBone, skeleton, true);
		}

		/// <summary>
		/// Return root bone in skeleton
		/// </summary>
		/// <param name="skeleton"></param>
		/// <returns></returns>
		public static Bone GetRootBoneInfo(this IeCoreEntities.Animation.Skeleton skeleton)
		{
			return skeleton.Bones.Find(x => x.ParentName == string.Empty);
		}

		private static void PrintPretty(string indent, Bone bone, IeCoreEntities.Animation.Skeleton skeleton, bool isLast)
		{
			Console.WriteLine(indent + "+- " + bone.Name);
			indent += isLast ? "   " : "|  ";

			List<Bone> childrenBones = skeleton.Bones.Where(x => x.ParentName == bone.Name).ToList();

			for (int i = 0; i < childrenBones.Count; i++)
				PrintPretty(indent, childrenBones[i], skeleton, i == childrenBones.Count - 1);
		}
	}
}
