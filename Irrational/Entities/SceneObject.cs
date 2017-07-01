//using System;
//using System.Collections.Generic;
//using OpenGL;

//namespace IrrationalSpace
//{
//	public class SceneObject : ISceneObject
//	{
//		private Vector3 shadingColor = new Vector3(1, 1, 1);

//        public Vector3 position{ get; set; }

//        public Vector3 scale{ get; set; }

//        public Vector3 rotation{ get; set; }

//		public Material mat{ get; set; }		

//		public Mesh mesh { get; set;}

//		public void SetMAterial()
//        {
//			Console.WriteLine(mat.shader.ProgramLog);
//            mat.shader.Use();
//            Console.WriteLine(mat.shader.ProgramLog);

//            mat.shader["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
			
//            mat.shader["alpha_str"].SetValue(1f);
//            mat.shader["color"].SetValue(shadingColor);

//            mat.shader["normalTexture"].SetValue(1);
//            mat.shader["enable_mapping"].SetValue(true);
//        }

//        public void ChangeTransform()
//        {
//            mat.shader["model_matrix"].SetValue(Matrix4.CreateRotationX(rotation.x)* Matrix4.CreateRotationY(rotation.y)* Matrix4.CreateRotationZ(rotation.z)
//                                             * Matrix4.CreateScaling(scale)
//                                             * Matrix4.CreateTranslation(position));
//        }
//	}
//}
