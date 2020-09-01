using System.Drawing.Imaging;
using System.Numerics;

namespace IeCoreEntites.Materials
{
    /// <summary>
    /// Determines texture asset.
    /// </summary>
    public class Texture : Asset
    {
        /// <summary>
        /// Id of texture.
        /// </summary>
        public int Id { get; set; } = -1;
        /// <summary>
        /// Texture data.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Texture resolution.
        /// </summary>
        public Vector2 TextureSize { get; set; }
        /// <summary>
        /// Texture wrapping mode.
        /// </summary>
        public TextureWrapping TextureWrapping { get; set; }

        /// <summary>
        /// Texture pixel format.
        /// </summary>
        public PixelFormat PixelFormat { get; set; } = PixelFormat.Format32bppArgb;

        /// <summary>
        /// <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public Texture(string name, string file):base(name,file)
        { }
    }
}
