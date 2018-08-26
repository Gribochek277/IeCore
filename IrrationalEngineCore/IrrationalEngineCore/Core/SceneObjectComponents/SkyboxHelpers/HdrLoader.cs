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

namespace Irrational.Core.SceneObjectComponents.SkyboxHelpers
{
    public class HdrLoader
    {
        public int[] LoadHdr(string _skyboxLocation)
        {
            int[] result = new int[5];

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
                captureRBO, 512);
            result[2] = 
                TransformHdrToCubemap(result[1],
                "environmentMap",
                new ShaderProg("vs_environment_to_irradiance.glsl", "fs_environment_to_irradiance.glsl", true),
                captureFBO,
                captureRBO,
                32);
            result[3] = TransformHdrToPrefilteredCubemap(result[1], new ShaderProg("vs_prefilter.glsl", "fs_prefilter.glsl", true),
            captureFBO,captureRBO, 128, 5);

            result[4] = CalculateBRDF(new ShaderProg("vs_brdf.glsl","fs_brdf.glsl", true), captureFBO, captureRBO, 512);

            return result;
        }

        private int LoadHdrImage(string pathToFile)
        {
            MagickImage img = new MagickImage(pathToFile);
            
            img.Flip();
            byte[] data = img.GetPixels().ToByteArray(0, 0, img.Width, img.Height, "RGB");
           
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, img.Width, img.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte, data);
     

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            return texID;
        }


        private int TransformHdrToCubemap(int textureId,string textureName, ShaderProg shader,int framebuffer, int renderbuffer, int size = 2048)
        {
           
            Cube cube = new Cube();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, size, size);
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
            GL.ActiveTexture(TextureUnit.Texture0);
            for (int i = 0; i < 6; ++i)
            {
                GL.UniformMatrix4(equirectangularToCubemapShader.GetUniform("view"), false, ref views[i]);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                                         TextureTarget.TextureCubeMapPositiveX + i, cubemap, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                cube.RenderCube(); // renders a 1x1 cube
            }

           
            equirectangularToCubemapShader.DisableVertexAttribArrays();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return cubemap;
        }

        private int TransformHdrToPrefilteredCubemap(int textureId, ShaderProg shader, int framebuffer, int renderbuffer, int size = 128, uint mipnapLevels = 5)
        {
           
            Cube cube = new Cube();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
           
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, renderbuffer);

            int cubemap = AllocatePrefilterCubemap(size);
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
            
            for(uint i=0; i< mipnapLevels;++i)
            {
                uint mipWidth = (uint)(size * Math.Pow(0.5f, i));
                uint mipHeight = (uint)(size * Math.Pow(0.5f, i));

                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, (int)mipWidth, (int)mipHeight);           
                GL.Viewport(0, 0, (int)mipWidth, (int)mipHeight);

                float roughness = (float)i / (float)(mipnapLevels - 1);
                helper.TryAddUniform1(roughness, "roughness", prefilteredShader);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
               // GL.Disable(EnableCap.FramebufferSrgb);
                //GL.ActiveTexture(TextureUnit.Texture0);
                for (uint j = 0; j < 6; ++j)
                {
                    GL.UniformMatrix4(prefilteredShader.GetUniform("view"), false, ref views[j]);
                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                                             TextureTarget.TextureCubeMapPositiveX + (int)j, cubemap, (int)i);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
              //      GL.Enable(EnableCap.FramebufferSrgb);
                    cube.RenderCube(); // renders a 1x1 cube
                   // Console.WriteLine(GL.CheckNamedFramebufferStatus(framebuffer, FramebufferTarget.Framebuffer));
                }


                prefilteredShader.DisableVertexAttribArrays();
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return cubemap;
        }


        private int CalculateBRDF( ShaderProg shader, int framebuffer, int renderbuffer, int size = 512)
        {
           int textureId = /*LoadBrdfImage("Resources/ibl_brdf_lut.png");*/ AllocateBrdf(size);
            Quad quad = new Quad();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
             RenderbufferStorage.DepthComponent24, size, size);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
             FramebufferAttachment.ColorAttachment0,
             TextureTarget.Texture2D,
             textureId, 0);
            ShaderProg brdfShader = shader;
            GL.UseProgram(brdfShader.ProgramID);
            brdfShader.EnableVertexAttribArrays();
            GL.Viewport(0, 0, size, size);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            quad.RenderQuad();
           
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            brdfShader.DisableVertexAttribArrays();
            
            return textureId;
        }

        int LoadBrdfImage(string filename)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(filename);
           
           int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Srgb8, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
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
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb16f, size, size, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
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

        public int AllocatePrefilterCubemap(int size = 128)
        {
            int prefilterMap = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, prefilterMap);

            for (int i = 0; i < 6; i++)
            {
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb16f, size, size, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

            return prefilterMap;
        }

        public int AllocateBrdf(int size = 512)
        {
            int brdfTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D,brdfTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rg16, size, size,0,OpenTK.Graphics.OpenGL.PixelFormat.Rg,
            PixelType.Float, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            return brdfTexture;
        }
    }
}
