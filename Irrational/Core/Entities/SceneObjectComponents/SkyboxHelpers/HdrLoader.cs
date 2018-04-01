using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;
using OpenTK.Graphics.OpenGL;

namespace Irrational.Core.Entities.SceneObjectComponents.SkyboxHelpers
{
    public class HdrLoader
    {
        public int LoadHdr(string _skyboxLocation)
        {

            if (_skyboxLocation == string.Empty)
            {
                throw new FileNotFoundException("Provided location is empty");
            }

            return LoadHdrImage(_skyboxLocation, PixelInternalFormat.Srgb8);
        }

        private int LoadHdrImage(string pathToFile, PixelInternalFormat textureColorspace)
        {
            Bitmap bitmap = null;
            using (MagickImage image = new MagickImage(pathToFile))
            {
                bitmap = image.ToBitmap();
            }

            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
           BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, textureColorspace, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);            

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);

           

            return texID;
        }
       
    }
}
