using System.Collections.Generic;
using Irrational.Core.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Core.Shaders;

namespace IrrationalEngineCore.Core.Shaders.Abstractions
{
    public interface IShaderImplementation : ILoadable
    {
         ShaderProg shaderProg {get;}
         Dictionary<string, Material> Materials { get; set; }
         Dictionary<string, int> Textures { get  ; set ; }
         void SetSpecificUniforms(IPipelineData pipelineData);
    }
}