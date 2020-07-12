using System.Collections.Generic;

namespace IeCoreEntites.Model
{
    /// <summary>
    /// Determines animation asset.
    /// </summary>
    public class Animation: Asset
    {
        /// <summary>
        /// Frame rate of animation
        /// </summary>
        public int FrameRate { get; set; }

        /// <summary>
        /// Collection of animation keys
        /// </summary>
        public List<Pose> Keys { get; set; } = new List<Pose>();

        /// <summary>
        /// <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public Animation(string name, string file): base(name, file)
        {           
        }

        //TODO: write extensions to export poses and animations
    }
}
