using System.Collections.Generic;
using Irrational.Core.Entities;
using Irrational.Core.Shaders;

namespace IrrationalEngineCore.Core.Shaders.Abstractions
{
    public interface IShaderImplementation
    {
         ShaderProg shaderProg {get;}
         Dictionary<string, Material> Materials { get; set; }
         Dictionary<string, int> Textures { get  ; set ; }
         void SetSpecificUniforms();
    }
}