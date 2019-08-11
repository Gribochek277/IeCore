using System.Collections.Generic;
using IrrationalEngineCore.Core.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.Shaders;

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