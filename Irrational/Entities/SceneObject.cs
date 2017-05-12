using System;
using System.Collections.Generic;
using OpenGL;

namespace IrrationalSpace
{
	public class SceneObject
	{
        public Texture diffuse,normal;
        public WavefrontModel mesh;
        public ShaderProgram program;
        private Vector3 shadingColor = new Vector3(1, 1, 1);
        public VBO<Vector2> modelUV;
        public VBO<Vector3> modelNormals;
        public VBO<Vector3> modelVertex;
        public VBO<Vector3> modelTangents;
        public VBO<int> modelElements;

        public Vector3 position;

        public Vector3 scale;

        public Vector3 rotation;

       

		public void SetMAterial(string diffuseTextureName,string normalTextureName,Scene scene, float alphaStr,string VertextShader, string FragmentsShader)
        {
            program =  new ShaderProgram(VertextShader, FragmentsShader);
            diffuse = new Texture(diffuseTextureName);
            normal = new Texture(normalTextureName);
            Console.WriteLine(program.ProgramLog);
            program.Use();
            Console.WriteLine(program.ProgramLog);

            //TODO : вынести в отдельеую функцию перемещения обхекта
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)ApplicationWindow.widght / ApplicationWindow.height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
			program["light_direction"].SetValue(scene.LightDirection);
			program["enable_lighting"].SetValue(scene.EnableLight);
			program["light_strenght"].SetValue(scene.LightStr);
            program["alpha_str"].SetValue(alphaStr);
            program["color"].SetValue(shadingColor);

            program["normalTexture"].SetValue(1);
            program["enable_mapping"].SetValue(true);
        }

        public SceneObject(string meshName,Vector3 _position,Vector3 _scale,Vector3 _rotation)
        {
            position = _position;
            scale = _scale;
            rotation = _rotation;
            mesh = ModelLoader.LoadModel(meshName);
            modelVertex = new VBO<Vector3>(mesh.vertices);//right
          
            List<int> elements = new List<int>();
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                elements.Add(i);
            }
            modelElements = new VBO<int>(elements.ToArray(), BufferTarget.ElementArrayBuffer);
            modelTangents = new VBO<Vector3>(ModelLoader.CalculateTangents(mesh.vertices, mesh.normals, elements.ToArray(), mesh.uvCoords));
            modelUV = new VBO<Vector2>(mesh.uvCoords);
            modelNormals = new VBO<Vector3>(mesh.normals);

        }

        public void ChangeTransform()
        {
            
            program["model_matrix"].SetValue(Matrix4.CreateRotationX(rotation.x) * Matrix4.CreateRotationY(rotation.y)* Matrix4.CreateRotationZ(rotation.z)
                                             * Matrix4.CreateScaling(scale)
                                             * Matrix4.CreateTranslation(position));
        }
	}
}
