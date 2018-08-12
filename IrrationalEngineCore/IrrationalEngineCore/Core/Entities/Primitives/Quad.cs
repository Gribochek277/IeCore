using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Entities.Primitives
{
    public class Quad
    {
        private int quadVAO = 0;
        private int quadVBO = 0;
        private float[] vertices = {
                    -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
                    -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                     1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
                     1.0f, -1.0f, 0.0f, 1.0f, 0.0f
                };

        public float[] Vertices { get {return vertices;} }
        public void RenderQuad()
        {
            // initialize (if necessary)
            if (quadVAO == 0)
            {               

                GL.GenVertexArrays(1, out quadVAO);
                GL.GenBuffers(1, out quadVBO);
               
                GL.BindVertexArray(quadVAO);
                GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                vertices, BufferUsageHint.StaticDraw);
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,
                5 * sizeof(float), 0); 
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false,
                5 * sizeof(float), 3 * sizeof(float)); 
            }
            // render Cube
            GL.BindVertexArray(quadVAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.BindVertexArray(0);
    }
    }
}