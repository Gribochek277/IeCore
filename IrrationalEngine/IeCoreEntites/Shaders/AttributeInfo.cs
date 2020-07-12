namespace IeCoreEntites.Shaders
{
    /// <summary>
    /// Contains information about shader attribute.
    /// </summary>
    public class AttributeInfo
	{
		/// <summary>
		/// Attribute name.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Attribute address.
		/// </summary>
		public int Address { get; set; } = -1;
		/// <summary>
		/// Size of attribute.
		/// </summary>
		public int Size = 0;
        /// <summary>
        /// Code of attribute type as integer value.
        /// </summary>
        public int Type;
	}
}
