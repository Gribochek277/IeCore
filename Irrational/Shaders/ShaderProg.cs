using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
namespace IrrationalSpace.Shaders
{
    public class ShaderProg
    {
        public int programId;
        int vertexShaderId;
        int fragmenShaderId;

        public int attribute_vcol;
        public int attribute_vpos;
        public int uniform_mview;

        public int vbo_position;
        public int vbo_color;
        public int vbo_mview;

		public Vector3[] vertdata;
        public Vector3[] coldata;
        public Matrix4[] mviewdata;

        public void InitShaderProgram(string vertexShader, string fragmentShader)
        {
            programId = GL.CreateProgram();

			vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
				new Vector3( 0.8f, -0.8f, 0f),
				new Vector3( 0f,  0.8f, 0f)};


			coldata = new Vector3[] { new Vector3(1f, 0f, 0f),
				new Vector3( 0f, 0f, 1f),
				new Vector3( 0f,  1f, 0f)};


			mviewdata = new Matrix4[]{
				Matrix4.Identity
			};

            ShaderEntity.LoadShader(vertexShader,ShaderType.VertexShader,programId,out vertexShaderId);
            ShaderEntity.LoadShader(fragmentShader, ShaderType.FragmentShader, programId, out fragmenShaderId);
            GL.LinkProgram(programId);
            Console.WriteLine(GL.GetProgramInfoLog(programId));

			attribute_vpos = GL.GetAttribLocation(programId, "vPosition");
			attribute_vcol = GL.GetAttribLocation(programId, "vColor");
			uniform_mview = GL.GetUniformLocation(programId, "modelview");

			if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
			{
				Console.WriteLine("Error binding attributes");
			}

			GL.GenBuffers(1, out vbo_position);
			GL.GenBuffers(1, out vbo_color);
			GL.GenBuffers(1, out vbo_mview);

			
		}
    }
}
