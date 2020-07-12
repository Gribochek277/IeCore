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
        /// <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public Texture(string name, string file):base(name,file)
        { }
    }
}
