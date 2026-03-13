using IeCoreEntities.Materials;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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
			int width = maxXCells * cellSize;
			int height = maxYCells * cellSize;

			using var image = new Image<Rgba32>(width, height);
			for (int y = 0; y < height; y++)
			{
				int yCell = y / cellSize;
				for (int x = 0; x < width; x++)
				{
					int xCell = x / cellSize;
					bool usePrimaryColor = ((xCell + yCell) % 2) == 0;
					image[x, y] = usePrimaryColor ? color1 : color2;
				}
			}

			byte[] rawBytes = new byte[width * height * 4];
			image.CopyPixelDataTo(rawBytes);

			return new Texture(
				$"CheckerboardTexture_resolution_{width}x{height}",
				InMemoryFileName)
			{
				TextureSize = new Vector2(width, height),
				Bytes = rawBytes
			};
		}
		}
	}
