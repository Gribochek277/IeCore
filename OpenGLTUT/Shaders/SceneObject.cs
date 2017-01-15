using System;
using System.Collections.Generic;
using OpenGL;

namespace IrrationalSpace
{
	public class SceneObject
	{
        public Texture texture;
        public WavefrontModel mesh;
        public ShaderProgram program;
        private Vector3 shadingColor = new Vector3(1, 1, 1);
        public VBO<Vector2> modelUV;
        public VBO<Vector3> modelNormals;
        public VBO<Vector3> modelVertex;
        public VBO<int> modelElements;

        public Vector3 position;

        public Vector3 scale;

        public Vector3 rotation;

        #region DefaultShaders
		public static string VertextShader = @"
                #version 130
                in vec3 vertexPosition;
                in vec3 vertexNormal;
                in vec2 vertexUV;
                out vec2 UV;
                out vec3 normal;


                uniform mat4 projection_matrix;
                uniform mat4 view_matrix;
                uniform mat4 model_matrix;

                void main(void)
                {
                normal = normalize(model_matrix*vec4(vertexNormal,0)).xyz;
                UV = vertexUV;
                gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition,1);
                }";
		public static string FragmentShader = @"
                #version 130
                uniform sampler2D texture; 
                uniform vec3 light_direction;
                uniform float light_strenght;
                uniform bool enable_lighting;
                uniform float alpha_str;
                uniform vec3 color;
                in vec3 normal;
                in vec2 UV;


                out vec4 fragment;
                void main(void)
                {
                float diffuse = max(dot(normal,light_direction)*light_strenght,0);
                float ambient = 0.3;
                float lighting = (enable_lighting?max(diffuse,ambient):1.0);
                vec4 sample = texture2D(texture,UV)*lighting;
                fragment = vec4(sample.xyz*color,sample.a*alpha_str);
                }";
        #endregion

        public void SetMAterial(string diffuseTextureName,bool enableLight, Vector3 lightDirection,float lightStr, float alphaStr,string VertextShader, string FragmentsShader)
        {
            
            program =  new ShaderProgram(VertextShader, FragmentShader);
            texture = new Texture(diffuseTextureName);
           
            program.Use();
            Console.WriteLine(program.ProgramLog);
            //TODO : вынести в отдельеую функцию перемещения обхекта
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)WindowPreferences.widght / WindowPreferences.height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
            //TODO : вынести сущность глобального освещения в отдельный класс
            program["light_direction"].SetValue(lightDirection);
            program["enable_lighting"].SetValue(enableLight);
            program["light_strenght"].SetValue(lightStr);
            program["alpha_str"].SetValue(alphaStr);
            program["color"].SetValue(shadingColor);



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
