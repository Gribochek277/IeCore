using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;
using IrrationaEngineCore.Utils.Extentions;
using Irrational.Core.Entities.Primitives;
using Irrational.Core.Renderer.OpenGL.Helpers;
using Irrational.Core.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Entities.SceneObjectComponents.SkyboxHelpers
{
    public class HdrLoader
    {
        public int[] LoadHdr(string _skyboxLocation)
        {
            int[] result = new int[4];

            if (_skyboxLocation == string.Empty)
            {
                throw new FileNotFoundException("Provided location is empty");
            }

            result[0] = LoadHdrImage(_skyboxLocation);
            int captureFBO, captureRBO;
            GL.GenFramebuffers(1, out captureFBO);
            GL.GenRenderbuffers(1, out captureRBO);
            result[1] = 
                TransformHdrToCubemap(result[0],
                "equirectangularMap",
                new ShaderProg("vs_equirectangular_to_cubemap.glsl", "fs_equirectangular_to_cubemap.glsl", true),
                captureFBO,
                captureRBO);
            result[2] = 
                TransformHdrToCubemap(result[1],
                "environmentMap",
                new ShaderProg("vs_environment_to_irradiance.glsl", "fs_environment_to_irradiance.glsl", true),
                captureFBO,
                captureRBO,
                32);
            result[3] = TransformHdrToPrefilteredCubemap(result[2], new ShaderProg("vs_prefilter.glsl", "fs_prefilter.glsl", true),
            captureFBO,captureRBO, 128, 5);

            return result;
        }

        private int LoadHdrImage(string pathToFile)
        {
            System.Drawing.Bitmap bitmap = null;
            using (MagickImage image = new MagickImage(pathToFile))
            {
                image.Flip();
                bitmap = image.ToBitmap();
            }

            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Srgb8, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);            

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);           

            return texID;
        }


        private int TransformHdrToCubemap(int textureId,string textureName, ShaderProg shader,int framebuffer, int renderbuffer, int size = 512)
        {
           
            Cube cube = new Cube();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Srgb8, size, size);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, renderbuffer);

            int cubemap = AllocateCubemap(size);
            Matrix4 projection = Matrix4.Identity;
            Matrix4[] views = new Matrix4[6];
            GenerateMatricesForMapHdrToCubemap(ref projection, ref views);

            ShaderProg equirectangularToCubemapShader = shader;
            GL.UseProgram(equirectangularToCubemapShader.ProgramID);
            equirectangularToCubemapShader.EnableVertexAttribArrays();
            GL.UniformMatrix4(equirectangularToCubemapShader.GetUniform("projection"), false, ref projection);
            UniformHelper helper = new UniformHelper();
            bool suc;
            if (textureName=="equirectangularMap")
                suc = helper.TryAddUniformTexture2D(textureId, textureName, equirectangularToCubemapShader, TextureUnit.Texture0);
            else
                suc = helper.TryAddUniformTextureCubemap(textureId, textureName, equirectangularToCubemapShader, TextureUnit.Texture0);

            GL.Viewport(0, 0, size, size);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.Disable(EnableCap.FramebufferSrgb);
            GL.ActiveTexture(TextureUnit.Texture0);
            for (int i = 0; i < 6; ++i)
            {
                GL.UniformMatrix4(equirectangularToCubemapShader.GetUniform("view"), false, ref views[i]);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                                         TextureTarget.TextureCubeMapPositiveX + i, cubemap, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Enable(EnableCap.FramebufferSrgb);
               // Console.WriteLine(GL.CheckNamedFramebufferStatus(framebuffer, FramebufferTarget.Framebuffer));
                cube.RenderCube(); // renders a 1x1 cube
            }

           
            equirectangularToCubemapShader.DisableVertexAttribArrays();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return cubemap;
        }

        private int TransformHdrToPrefilteredCubemap(int textureId, ShaderProg shader, int framebuffer, int renderbuffer, int size = 128, int mipnapLevels = 5)
        {
           
            Cube cube = new Cube();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Srgb8, size, size);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, renderbuffer);

            int cubemap = AllocateCubemap(size);
            Matrix4 projection = Matrix4.Identity;
            Matrix4[] views = new Matrix4[6];
            GenerateMatricesForMapHdrToCubemap(ref projection, ref views);

            ShaderProg prefilteredShader = shader;
            GL.UseProgram(prefilteredShader.ProgramID);
            prefilteredShader.EnableVertexAttribArrays();
            GL.UniformMatrix4(prefilteredShader.GetUniform("projection"), false, ref projection);
            UniformHelper helper = new UniformHelper();
            bool suc;
                suc = helper.TryAddUniformTextureCubemap(textureId, "environmentMap", prefilteredShader, TextureUnit.Texture0);
                Console.WriteLine(suc);
            for(int i=0; i< mipnapLevels;i++)
            {
                Console.WriteLine((int)(size * Math.Pow(0.5f, i)));
                GL.Viewport(0, 0, (int)(size * Math.Pow(0.5f, i)), (int)(size * Math.Pow(0.5f, i)));
                float roughness = (float)i / (float)(mipnapLevels - 1);
                Console.WriteLine(roughness);
                helper.TryAddUniform1(roughness, "roughness", prefilteredShader);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
                GL.Disable(EnableCap.FramebufferSrgb);
                GL.ActiveTexture(TextureUnit.Texture0);
                for (int j = 0; j < 6; ++j)
                {
                    GL.UniformMatrix4(prefilteredShader.GetUniform("view"), false, ref views[j]);
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                                             TextureTarget.TextureCubeMapPositiveX + j, cubemap, i);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.Enable(EnableCap.FramebufferSrgb);
                    cube.RenderCube(); // renders a 1x1 cube
                   // Console.WriteLine(GL.CheckNamedFramebufferStatus(framebuffer, FramebufferTarget.Framebuffer));
                }


                prefilteredShader.DisableVertexAttribArrays();
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return cubemap;
        }


        private void GenerateMatricesForMapHdrToCubemap(ref Matrix4 projection, ref Matrix4[] views)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(1.5708f, 1.0f, 0.1f, 10.0f);
            views = new Matrix4[]{
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(1.0f,  0.0f,  0.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(-1.0f,  0.0f,  0.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  1.0f,  0.0f),new Vector3(0.0f,  0.0f,  1.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f, -1.0f,  0.0f),new Vector3(0.0f,  0.0f, -1.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  0.0f,  1.0f),new Vector3(0.0f, -1.0f,  0.0f)),
                    Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f,  0.0f, -1.0f),new Vector3(0.0f, -1.0f,  0.0f))
                };
        }

        public int AllocateCubemap(int size = 512)
        {
            int envCubemap = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, envCubemap);

            for (int i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Srgb8, size, size, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMaxLevel, 0);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return envCubemap;
        }
    }
}
