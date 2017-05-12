using System;
using OpenGL;
namespace IrrationalSpace
{
	public class Mesh
	{
		public VBO<Vector2> modelUV;
		public VBO<Vector3> modelNormals;
		public VBO<Vector3> modelVertex;
		public VBO<Vector3> modelTangents;
		public VBO<int> modelElements;
	}
}
