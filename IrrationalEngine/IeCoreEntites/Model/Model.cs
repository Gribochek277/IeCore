using System.Collections.Generic;
using System.Numerics;

namespace IeCoreEntites.Model
{
    /// <summary>
    /// Determines Model asset.
    /// </summary>
    public class Model: Asset
    {
        /// <summary>
        /// Id of VAO associated with this model.
        /// </summary>
        public int VertexArrayObjectId { get; set; } = -1;
        /// <summary>
        /// Id of VBO associated with this model.
        /// </summary>
        public int VertexBufferObjectId { get; set; } = -1;
        /// <summary>
        /// Id of Element Buffer associated with this model.
        /// </summary>
        public int ElementBufferId { get; set; } = -1;
        /// <summary>
        /// TextureColor
        /// </summary>
        public static Vector4 TextureColor = Vector4.One;
        /// <summary>
        /// PointColor
        /// </summary>
        public static Vector4 PointColor = new Vector4(0.1f, 0.0f, 0.0f, 1.0f);
        /// <summary>
        /// EdgeColor
        /// </summary>
        public static Vector4 EdgeColor = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
        /// <summary>
        /// FaceColor
        /// </summary>
        public static Vector4 FaceColor = new Vector4(0.6f, 0.6f, 1.0f, 1.0f);

        /// <summary>
        /// Contains collection of meshes which belongs to this model.
        /// </summary>
        public List<Mesh> Meshes { get; private set; } = new List<Mesh>();
        /// <summary>
        /// Contains collection of poses which belongs to this model.
        /// </summary>
        public List<Pose> Poses { get; private set; } = new List<Pose>();
        /// <summary>
        /// Contains collection of animations which belong to this model.
        /// </summary>
        public List<Animation> Animations { get; private set; } = new List<Animation>();

        /// <summary>
        /// Ctor. <inheritdoc cref="Asset"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public Model(string name, string file) : base(name, file)
        { }
    }
}
