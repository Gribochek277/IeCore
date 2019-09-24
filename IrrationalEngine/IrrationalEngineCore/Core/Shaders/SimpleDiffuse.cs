using System.Collections.Generic;
using System.Linq;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.Renderer.OpenGL.Helpers;
using IrrationalEngineCore.Core.Shaders.Abstractions;
using OpenTK.Graphics.OpenGL;

namespace IrrationalEngineCore.Core.Shaders
{
    public class SimpleDiffuse : IShaderImplementation
    {
        public ShaderProg shaderProg { get; }

        private const string maintexture = "maintexture";
        private const string isTextured = "isTextured";

        private readonly UniformHelper _uniformHelper;

        public Dictionary<string, Material> Materials { get; set; } = new Dictionary<string, Material>();
        public Dictionary<string, int> Textures { get; set; } = new Dictionary<string, int>();

        public SimpleDiffuse() {
            shaderProg = new ShaderProg("diffuse_vs.glsl", "diffuse_fs.glsl", true);
            _uniformHelper = new UniformHelper();
        }
        public void OnLoad()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }

        public void SetSpecificUniforms(IPipelineData pipelineData)
        {
            int texId;
            bool isTexPresent = Textures.TryGetValue(Materials.FirstOrDefault().Value.DiffuseMap, out texId);

            if (isTexPresent)
            {
                _uniformHelper.TryAddUniformTexture2D(texId, maintexture, shaderProg, TextureUnit.Texture0);
                _uniformHelper.TryAddUniform1(1, isTextured, shaderProg);
            }
            else
            {
                _uniformHelper.TryAddUniform1(0, isTextured, shaderProg);
            }
            
        }
    }
}
