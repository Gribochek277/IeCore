using System.Numerics;

namespace IeCoreEntities.Materials
{
    /// <summary>
    /// Contains set of properties and textures for this material asset.
    /// </summary>
    public class Material : Asset
    {
        /// <summary>
        /// AmbientColor
        /// </summary>
        public Vector4 AmbientColor { get; set; }
        /// <summary>
        /// DiffuseColor
        /// </summary>
        public Vector4 DiffuseColor { get; set; }
        /// <summary>
        /// SpecularColor
        /// </summary>
        public Vector4 SpecularColor { get; set; }
        /// <summary>
        /// EmissionColor
        /// </summary>
        public Vector4 EmissionColor { get; set; }
        /// <summary>
        /// Alpha
        /// </summary>
        public float Alpha { get; set; }
        /// <summary>
        /// Shininess
        /// </summary>
        public float Shininess { get; set; }
        /// <summary>
        /// IlluminationMode
        /// </summary>
        public int IlluminationMode { get; set; }
        /// <summary>
        /// AmbientTexture
        /// </summary>
        public Texture AmbientTexture { get; set; }
        /// <summary>
        /// DiffuseTexture
        /// </summary>
        public Texture DiffuseTexture { get; set; }
        /// <summary>
        /// SpecularTexture
        /// </summary>
        public Texture SpecularTexture { get; set; }
        /// <summary>
        /// AlphaTexture
        /// </summary>
        public Texture AlphaTexture { get; set; }
        /// <summary>
        /// BumpTexture
        /// </summary>
        public Texture BumpTexture { get; set; }
        /// <summary>
        /// NormalTexture
        /// </summary>
        public Texture NormalTexture { get; set; }
        /// <summary>
        /// HeightTexture
        /// </summary>
        public Texture HeightTexture { get; set; }
        /// <summary>
        /// <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public Material(string name, string file): base(name, file)
        {           
        }
    }
}
