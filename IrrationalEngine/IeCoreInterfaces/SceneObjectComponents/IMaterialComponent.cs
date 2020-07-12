using IeCoreEntites.Materials;
using IeCoreInterfaces.Core;
using IeCoreInterfaces.Shaders;
using System.Collections.Generic;

namespace IeCoreInterfaces.SceneObjectComponents
{
    /// <summary>
    /// Represents scene object component which responsible for materials.
    /// </summary>
    public interface IMaterialComponent : ISceneObjectComponent
    {
        /// <summary>
        /// Dictionary of loaded materials which belong to this component
        /// </summary>
        Dictionary<string, Material> materials { get; }
        /// <summary>
        /// Shader program which related to this component.
        /// </summary>
        IShaderProgram ShaderProgram { get;}
    }
}
