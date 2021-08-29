using IeCoreEntities.Animation;
using System.Collections.Generic;

namespace IeCoreEntities.Model
{
	/// <summary>
	/// Determines mesh, part of <see cref="Model"/> asset.
	/// After Mesh is being loaded to GPU memory and will not be changed further it cloud be cleared from memory.
	/// </summary>
	public class Mesh
	{
		/// <summary>
		/// Name of mesh.
		/// </summary>
		public string Name { get; set; }
		//TODO: is this required here?
		/// <summary>
		/// Material name which belong to this mesh.
		/// </summary>
		public string MaterialName { get; set; }
		/// <summary>
		/// Vertices
		/// </summary>
		// TODO: Vertices includes texture coordinates now, consider to split this on two texture collections.
		public List<Vertex> Vertices { get; set; }
		/// <summary>
		/// Elements (some tutorials use indices's definition) 
		/// </summary>
		public List<uint> Elements { get; set; }

		/// <summary>
		/// ElementOffset
		/// </summary>
		public int ElementOffset { get; set; }
		/// <summary>
		/// ElementCount
		/// </summary>
		public int ElementCount { get; set; }

		/// <summary>
		/// RenderMode <see cref="RenderMode"/>
		/// </summary>
		public RenderMode RenderMode { get; set; } = RenderMode.Texture;

		/// <summary>
		/// All transformation related data of mesh.
		/// </summary>
		public Transform Transform { get; set; } = new Transform();

		/// <summary>
		/// Skeleton which belong to the model.
		/// </summary>
		public Skeleton Skeleton { get; set; } = new Skeleton();

		/// <summary>
		/// <inheritdoc cref="object.ToString()"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("[Mesh: {0}, Elements=({1},{2})]", Name, ElementOffset, ElementCount);
		}
	}
}
