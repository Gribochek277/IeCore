namespace IeCoreInterfaces.Behaviour
{
    /// <summary>
    /// Determines that class should support loading behaviour.
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Calls on loading.
        /// </summary>
        void OnLoad();

        /// <summary>
        /// calls on unloading,
        /// </summary>
        void OnUnload();
    }
}
