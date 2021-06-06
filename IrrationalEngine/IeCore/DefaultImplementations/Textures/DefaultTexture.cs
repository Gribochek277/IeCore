using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using IeCoreEntities.Materials;

namespace IeCore.DefaultImplementations.Textures
{
    public static class DefaultTexture
    {
        private const string InMemoryFileName = "InMemoryTexture.jpg";

        public static Texture CreateDefaultCheckerboard(int resolution, int cellSize = 32)
        {
            int maxCells = resolution / cellSize;
            return CreateDefaultCheckerboard(maxCells, maxCells, cellSize);
        }
        public static Texture CreateDefaultCheckerboard(int resolution, Color color1,
        Color color2, int cellSize = 32)
        {
            int maxCells = resolution / cellSize;
            return CreateDefaultCheckerboard(maxCells, maxCells, cellSize, color1, color2);
        }
        public static Texture CreateDefaultCheckerboard(int maxXCells,
        int maxYCells,
        int cellSize)
        {
            return CreateDefaultCheckerboard(maxXCells, maxYCells, cellSize, Color.White, Color.Purple);
        }
        public static Texture CreateDefaultCheckerboard(
        int maxXCells,
        int maxYCells,
        int cellSize,
        Color color1,
        Color color2)
        {
            using (var bmp = new Bitmap(maxXCells*cellSize, maxYCells * cellSize))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (var blackBrush = new SolidBrush(color1))
                    using (var whiteBrush = new SolidBrush(color2))
                    {
                        for (var i = 0; i < maxXCells; i++)
                        {
                            for (var j = 0; j < maxYCells; j++)
                            {
                                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                                    g.FillRectangle(blackBrush, i * cellSize, j * cellSize, cellSize, cellSize);
                                else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                                    g.FillRectangle(whiteBrush, i * cellSize, j * cellSize, cellSize, cellSize);
                            }
                        }
                    }
                }
                Texture result = null;

                using (var memStream = new MemoryStream()) {
                    bmp.Save(memStream, ImageFormat.Png);
                    BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    result = new Texture(
                        $"CheckerboardTexture_resolution_{maxXCells * cellSize}x{maxYCells * cellSize}", InMemoryFileName)
                    {
                        TextureSize = new Vector2(maxXCells * cellSize, maxYCells * cellSize)
                    };

                    var data = new byte[Math.Abs(bitmapData.Stride * bitmapData.Height)];
                    Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
                    result.Bytes = data;
                    bmp.UnlockBits(bitmapData);
                }

                return result;
            }
        }
    }
}
