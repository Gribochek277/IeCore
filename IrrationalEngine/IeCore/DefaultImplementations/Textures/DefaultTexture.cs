using IeCoreEntities.Materials;
using System.IO;
using System.Numerics;
using SixLabors.ImageSharp;

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

		private static Texture CreateDefaultCheckerboard(
		int maxXCells,
		int maxYCells,
		int cellSize,
		Color color1,
		Color color2)
		{

			Texture result = null;
			using (Image image =
			       Image.Load(
				       $"Resources{Path.DirectorySeparatorChar}Textures{Path.DirectorySeparatorChar}checkerboard.png"))
			{
				using (var memStream = new MemoryStream())
				{
					image.SaveAsPng(memStream);


					//	BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					result = new Texture(
						$"CheckerboardTexture_resolution_{maxXCells * cellSize}x{maxYCells * cellSize}",
						InMemoryFileName)
					{
						TextureSize = new Vector2(maxXCells * cellSize, maxYCells * cellSize)
					};

					result.Bytes = memStream.ToArray();
				}
				
			}
			return result;
		}
		}
	}
