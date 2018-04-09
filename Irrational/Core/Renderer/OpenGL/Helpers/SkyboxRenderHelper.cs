using Irrational.Core.Entities;
using Irrational.Core.Entities.SceneObjectComponents;
using Irrational.Core.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Irrational.Core.Renderer.OpenGL.Helpers
{
    public class SkyboxRenderHelper
    {
        static int envCubemap = 0;
        static bool skyboxLoaded = false;
        static UniformHelper uniformHelper = new UniformHelper();
        static int captureFBO, captureRBO;
        static ShaderProg equirectangularToCubemapShader = new ShaderProg("vs_equirectangular_to_cubemap.glsl", "fs_equirectangular_to_cubemap.glsl", true);



        public static int RenderCubemapSkybox(Matrix4 view, Matrix4 projection, SceneObject skybox)
        {
            
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);
            SkyboxSceneObjectComponent skyboxComponent = (SkyboxSceneObjectComponent)skybox.components["SkyboxSceneObjectComponent"];
            MeshSceneObjectComponent skyboxMeshComponent = (MeshSceneObjectComponent)skybox.components["MeshSceneObjectComponent"];
            GL.UseProgram(skyboxComponent.Shader.ProgramID);
            skyboxComponent.Shader.EnableVertexAttribArrays();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("projection"), false, ref projection);
            Matrix4 clearTraslationViewMatrix = view.ClearTranslation();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("view"), false, ref clearTraslationViewMatrix);
            new UniformHelper().TryAddUniformTextureCubemap(skyboxComponent.TexId, "skybox", skyboxComponent.Shader, TextureUnit.Texture0);           
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.DrawElements(BeginMode.Triangles, skyboxMeshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, 0 * sizeof(uint));            
            GL.DepthFunc(DepthFunction.Less);
            skyboxComponent.Shader.DisableVertexAttribArrays();
            return skyboxMeshComponent.ModelMesh.IndiceCount; 
        }

        public static int RenderHdrToCubemapSkybox(Matrix4 view, Matrix4 projection, SceneObject skybox)
        {
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);
            SkyboxSceneObjectComponent skyboxComponent = (SkyboxSceneObjectComponent)skybox.components["SkyboxSceneObjectComponent"];
            MeshSceneObjectComponent skyboxMeshComponent = (MeshSceneObjectComponent)skybox.components["MeshSceneObjectComponent"];
            GL.UseProgram(skyboxComponent.Shader.ProgramID);
            skyboxComponent.Shader.EnableVertexAttribArrays();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("projection"), false, ref projection);
            Matrix4 clearTraslationViewMatrix = view.ClearTranslation();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("view"), false, ref clearTraslationViewMatrix);
            bool isSetted =  new UniformHelper().TryAddUniformTextureCubemap(envCubemap, "environmentMap", skyboxComponent.Shader, TextureUnit.Texture0);
            if (!isSetted)
            {
                Console.WriteLine("wrong uniform");
            }
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.DrawElements(BeginMode.Triangles, skyboxMeshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, 0 * sizeof(uint));
            GL.DepthFunc(DepthFunction.Less);
            skyboxComponent.Shader.DisableVertexAttribArrays();
            return skyboxMeshComponent.ModelMesh.IndiceCount;
        }


        private static void InitBuffersAndCubemap()
        {
            if (!skyboxLoaded)
            {
                GL.GenFramebuffers(1, out captureFBO);
                GL.GenRenderbuffers(1, out captureRBO);

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, captureFBO);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, captureRBO);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, 512, 512);
                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, captureRBO);

                envCubemap = GenerateEmptyCubemap();

                skyboxLoaded = true;
            }
        }

            public static void GenerateCubemapFromHdr(SceneObject skybox)
        {
                SkyboxSceneObjectComponent skyboxComponent = (SkyboxSceneObjectComponent)skybox.components["SkyboxSceneObjectComponent"];

            InitBuffersAndCubemap();
            Matrix4 captureProjection = Matrix4.CreatePerspectiveFieldOfView(1.5708f, 1.0f, 0.1f, 10.0f);
            Matrix4[] captureViews = {
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(1.0f,  0.0f,  0.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(-1.0f,  0.0f,  0.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  1.0f,  0.0f),new Vector3(0.0f,  0.0f,  1.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f, -1.0f,  0.0f),new Vector3(0.0f,  0.0f, -1.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  0.0f,  1.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  0.0f, -1.0f),new Vector3(0.0f, -1.0f,  0.0f))
                };

            GL.UseProgram(equirectangularToCubemapShader.ProgramID);
            equirectangularToCubemapShader.EnableVertexAttribArrays();
            GL.UniformMatrix4(equirectangularToCubemapShader.GetUniform("projection"), false, ref captureProjection);
            uniformHelper.TryAddUniformTexture2D(skyboxComponent.TexId, "equirectangularMap", equirectangularToCubemapShader, TextureUnit.Texture0);

            GL.Viewport(0, 0, 512, 512);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, captureFBO);
            GL.BindTexture(TextureTarget.TextureCubeMap, envCubemap);
            GL.Disable(EnableCap.FramebufferSrgb);
            GL.ActiveTexture(TextureUnit.Texture0);
            for (int i = 0; i < 6; ++i)
            {
                GL.ClearColor(Color.Violet);

                GL.UniformMatrix4(equirectangularToCubemapShader.GetUniform("view"), false, ref captureViews[i]);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                                         TextureTarget.TextureCubeMapPositiveX + i, envCubemap, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                RenderCube(); // renders a 1x1 cube


                Console.WriteLine(GL.CheckNamedFramebufferStatus(captureFBO, FramebufferTarget.Framebuffer));
            }

            equirectangularToCubemapShader.DisableVertexAttribArrays();
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, 800, 600);

        }

        public static int GenerateEmptyCubemap(int sizeX = 512, int sizeY = 512)
        {
            int envCubemap = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, envCubemap);
           
            for (int i=0;i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb16f, sizeX, sizeY, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);

            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMaxLevel, 0);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return envCubemap;
        }

        static int cubeVAO = 0;
        static int cubeVBO = 0;
        public static void RenderCube()
        {
            // initialize (if necessary)
            if (cubeVAO == 0)
            {
                float[] vertices = {
            // back face
            -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 0.0f, 0.0f, // bottom-left
             1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 1.0f, 1.0f, // top-right
             1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 1.0f, 0.0f, // bottom-right         
             1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 1.0f, 1.0f, // top-right
            -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 0.0f, 0.0f, // bottom-left
            -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f, 0.0f, 1.0f, // top-left
            // front face
            -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f, // bottom-left
             1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, 0.0f, // bottom-right
             1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, 1.0f, // top-right
             1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, 1.0f, // top-right
            -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, 1.0f, // top-left
            -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f, // bottom-left
            // left face
            -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f, 1.0f, 0.0f, // top-right
            -1.0f,  1.0f, -1.0f, -1.0f,  0.0f,  0.0f, 1.0f, 1.0f, // top-left
            -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, 0.0f, 1.0f, // bottom-left
            -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, 0.0f, 1.0f, // bottom-left
            -1.0f, -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, 0.0f, 0.0f, // bottom-right
            -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f, 1.0f, 0.0f, // top-right
            // right face
             1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f, 1.0f, 0.0f, // top-left
             1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, 0.0f, 1.0f, // bottom-right
             1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  0.0f, 1.0f, 1.0f, // top-right         
             1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, 0.0f, 1.0f, // bottom-right
             1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f, 1.0f, 0.0f, // top-left
             1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  0.0f, 0.0f, 0.0f, // bottom-left     
            // bottom face
            -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f, 0.0f, 1.0f, // top-right
             1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f, 1.0f, 1.0f, // top-left
             1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f, 1.0f, 0.0f, // bottom-left
             1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f, 1.0f, 0.0f, // bottom-left
            -1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f, 0.0f, 0.0f, // bottom-right
            -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f, 0.0f, 1.0f, // top-right
            // top face
            -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f, 0.0f, 1.0f, // top-left
             1.0f,  1.0f , 1.0f,  0.0f,  1.0f,  0.0f, 1.0f, 0.0f, // bottom-right
             1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f, 1.0f, 1.0f, // top-right     
             1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f, 1.0f, 0.0f, // bottom-right
            -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f, 0.0f, 1.0f, // top-left
            -1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f, 0.0f, 0.0f  // bottom-left        
        };
                GL.GenVertexArrays(1, out cubeVAO);
                GL.GenBuffers(1, out cubeVBO);
                // fill buffer
                GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 288 * sizeof(float), vertices, BufferUsageHint.StaticDraw);
                // link vertex attributes
                GL.BindVertexArray(cubeVAO);
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (3 * sizeof(float)));
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (6 * sizeof(float)));
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
            }
            // render Cube
            GL.BindVertexArray(cubeVAO);
            //GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0 * sizeof(uint));
            GL.DrawArrays(BeginMode.Triangles, 0, 36);
            GL.BindVertexArray(0);
        }
    }
}
