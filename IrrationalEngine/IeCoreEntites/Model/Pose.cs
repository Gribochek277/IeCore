namespace IeCoreEntites.Model
{
    /// <summary>
    /// Determines pose asset.
    /// </summary>
    public class Pose : Asset
    {
        /// <summary>
        /// Ctor. <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        public Pose(string name):base(name, string.Empty)
        { }
        /// <summary>
        /// Clones Pose asset.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Pose Clone(string name)
        {
            return new Pose(name);
        }
    }
}
