using System;
using System.Collections.Generic;
using OpenGL;

namespace IrrationalSpace
{
	public class SceneObject : ISceneObject
	{
        public Texture diffuse,normal;
        public WavefrontModel mesh;
        public ShaderProgram program;
		private Vector3 shadingColor = new Vector3(1, 1, 1);

        public Vector3 position{ get; set; }

        public Vector3 scale{ get; set; }

        public Vector3 rotation{ get; set; }

		public Material mat{ get; set; }

		public Scene scene { get; set; }

		public Mesh model { get; set;}

		public void SetMAterial()
        {
			program =  mat.shader;
			diffuse = mat.diffuse;
			normal = mat.normal;
            Console.WriteLine(program.ProgramLog);
            program.Use();
            Console.WriteLine(program.ProgramLog);

            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
			program["light_direction"].SetValue(scene.LightDirection);
			program["enable_lighting"].SetValue(scene.EnableLight);
			program["light_strenght"].SetValue(scene.LightStr);
            program["alpha_str"].SetValue(1f);
            program["color"].SetValue(shadingColor);

            program["normalTexture"].SetValue(1);
            program["enable_mapping"].SetValue(true);
        }

        public SceneObject(string meshName,Vector3 _position,Vector3 _scale,Vector3 _rotation)
        {
			//TODO : change the way of initialisation
            position = _position;
            scale = _scale;
            rotation = _rotation;
            mesh = ModelLoader.LoadModel(meshName);
			this.model = new Mesh();
            model.modelVertex = new VBO<Vector3>(mesh.vertices);//right
          
            List<int> elements = new List<int>();
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                elements.Add(i);
            }
            model.modelElements = new VBO<int>(elements.ToArray(), BufferTarget.ElementArrayBuffer);
            model.modelTangents = new VBO<Vector3>(ModelLoader.CalculateTangents(mesh.vertices, mesh.normals, elements.ToArray(), mesh.uvCoords));
            model.modelUV = new VBO<Vector2>(mesh.uvCoords);
            model.modelNormals = new VBO<Vector3>(mesh.normals);

        }

        public void ChangeTransform()
        {
            
            program["model_matrix"].SetValue(Matrix4.CreateRotationX(rotation.x) * Matrix4.CreateRotationY(rotation.y)* Matrix4.CreateRotationZ(rotation.z)
                                             * Matrix4.CreateScaling(scale)
                                             * Matrix4.CreateTranslation(position));
        }
	}
}
