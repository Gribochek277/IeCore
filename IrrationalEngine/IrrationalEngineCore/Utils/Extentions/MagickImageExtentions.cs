using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IrrationaEngineCore.Utils.Extentions
{
    public static class MagickImageExtentions
    {
        public static Bitmap ToBitmap(this MagickImage image)
        {

            string mapping = "BGR";
            PixelFormat format = PixelFormat.Format24bppRgb;

            try
            {
                if (image.ColorSpace != ColorSpace.sRGB)
                {
                    image.ColorSpace = ColorSpace.sRGB;
                }

                if (image.HasAlpha)
                {
                    mapping = "BGRA";
                    format = PixelFormat.Format32bppArgb;
                }

                using (IPixelCollection pixels = image.GetPixelsUnsafe())
                {
                    Bitmap bitmap = new Bitmap(image.Width, image.Height, format);
                    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, format);
                    IntPtr destination = data.Scan0;
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte[] bytes = pixels.ToByteArray(0, y, bitmap.Width, 1, mapping);
                        Marshal.Copy(bytes, 0, destination, bytes.Length);

                        destination = new IntPtr(destination.ToInt64() + data.Stride);
                    }

                    bitmap.UnlockBits(data);
                    return bitmap;
                }
            }
            finally
            {
                    image.Dispose();
            }
        }
    }
}
