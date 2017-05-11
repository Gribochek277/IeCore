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

        #region DefaultShaders
		public static string VertextShader = @"
                #version 130
                in vec3 vertexPosition;
in vec3 vertexNormal;
in vec3 vertexTangent;
in vec2 vertexUV;
uniform vec3 light_direction;
out vec3 normal;
out vec2 uv;
out vec3 light;
uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;
uniform bool enable_mapping;
void main(void)
{
    normal = normalize((model_matrix * vec4(floor(vertexNormal), 0)).xyz);
    uv = vertexUV;
    mat3 tbnMatrix = mat3(vertexTangent, cross(vertexTangent, normal), normal);
    light = (enable_mapping ? light_direction * tbnMatrix : light_direction);
    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}";
		public static string FragmentShader = @"
                #version 130
                uniform sampler2D texture; 
                uniform sampler2D normalTexture;

                uniform bool enable_mapping;
               
                uniform float light_strenght;
                uniform bool enable_lighting;

                uniform mat4 model_matrix;

                uniform float alpha_str;
                uniform vec3 color;

                in vec3 normal;
                in vec2 UV;
                in vec3 light;

                out vec4 fragment;
                void main(void)
                {

                vec3 fragmentNormal = texture2D(normalTexture, uv).xyz * 2 -1;
                vec3 selectedNormal = (enable_mapping ? fragmentNormal : normal);

                float diffuse = max(dot(selectedNormal,light)*light_strenght,0);
                float ambient = 0.3;
                float lighting = (enable_lighting ? max(diffuse,ambient):1.0);

                vec4 sample = texture2D(texture,UV)*lighting;
                fragment = vec4(sample.xyz*color,sample.a*alpha_str);

                }";

        public static string FragmentShader2 = @"
#version 130
uniform sampler2D colorTexture;
uniform sampler2D normalTexture;
uniform bool enable_lighting;
uniform mat4 model_matrix;
uniform bool enable_mapping;
 uniform float light_strenght;
in vec3 normal;
in vec2 uv;
in vec3 light;
uniform float alpha_str;

uniform vec3 color;
out vec4 fragment;
void main(void)
{
    vec3 fragmentNormal = texture2D(normalTexture, uv).xyz * 2 - 1;
    vec3 selectedNormal = (enable_mapping ? fragmentNormal : normal);
    float diffuse = max(dot(selectedNormal, light)*light_strenght, 0);
    float ambient = 0.3;
    float lighting = (enable_lighting ? max(diffuse, ambient) : 1);
vec4 sample = lighting * texture2D(colorTexture, uv);
   fragment = vec4(sample.xyz*color, sample.a*alpha_str);
}
";
        #endregion

        public void SetMAterial(string diffuseTextureName,string normalTextureName,bool enableLight, Vector3 lightDirection,float lightStr, float alphaStr,string VertextShader, string FragmentsShader)
        {
            	

            program =  new ShaderProgram(VertextShader, FragmentShader2);
            diffuse = new Texture(diffuseTextureName);
            normal = new Texture(normalTextureName);
            Console.WriteLine(program.ProgramLog);
            program.Use();
            Console.WriteLine(program.ProgramLog);

            //TODO : вынести в отдельеую функцию перемещения обхекта
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)ApplicationWindow.widght / ApplicationWindow.height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
            //TODO : вынести сущность глобального освещения в отдельный класс
            program["light_direction"].SetValue(lightDirection);
            program["enable_lighting"].SetValue(enableLight);
            program["light_strenght"].SetValue(lightStr);
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
