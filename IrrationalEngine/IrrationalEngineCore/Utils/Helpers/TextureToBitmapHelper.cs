using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
namespace IrrationalEngineCore.Utils.Helpers
{
    public static class TextureToBitmapHelper
    {
        public static void GetBitmap(int TextureId,int size, string path)
        {
            int fboId;
            GL.Ext.GenFramebuffers(1, out fboId);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboId);
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, TextureId, 0);

            Bitmap b = new Bitmap(size, size);
            var bits = b.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.ReadPixels(0, 0, size, size, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
            GL.Ext.DeleteFramebuffers(1, ref fboId);
            b.UnlockBits(bits);

            b.RotateFlip(RotateFlipType.RotateNoneFlipY);

            b.Save(path);
        }
    }
}
