using System.Collections.Generic;
using System.Linq;
using Irrational.Core.Entities;
using Irrational.Core.Renderer.OpenGL.Helpers;
using Irrational.Core.Shaders;
using IrrationalEngineCore.Core.Shaders.Abstractions;
using OpenTK.Graphics.OpenGL;

namespace IrrationalEngineCore.Core.Shaders
{
    public class Pbr : IShaderImplementation
    {
        public ShaderProg shaderProg {get;}

        public enum ShaderType { GltfWithMaps, OnlyAlbedoAndNormalMap }

        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        public Dictionary<string, Material> Materials { get { return _materials; } set { _materials = value; } }

        private Dictionary<string, int> _textures = new Dictionary<string, int>();      

        public Dictionary<string, int> Textures { get { return _textures; } set { _textures = value; } }

        private UniformHelper _uniformHelper;

        public Pbr(ShaderType shaderType)
        {
            switch(shaderType)
            {
                case ShaderType.GltfWithMaps:
                {
                    shaderProg =  new ShaderProg("vs_norm.glsl", "fs_PBR_gltf_pipeline_with_maps.glsl", true);
                    break;
                }
                case ShaderType.OnlyAlbedoAndNormalMap:
                {
                    shaderProg =  new ShaderProg("vs_norm.glsl", "fs_PBR.glsl", true);
                    break;
                }
            }
            _uniformHelper = new UniformHelper();
        }
        public void SetSpecificUniforms()
        {
                int texId = _textures[Materials.FirstOrDefault().Value.DiffuseMap];
                int normId = _textures[Materials.FirstOrDefault().Value.NormalMap];
                int metRoughId = _textures[Materials.FirstOrDefault().Value.MetallicRoughness];
                int ambientId = _textures[Materials.FirstOrDefault().Value.AmbientMap];

             _uniformHelper.TryAddUniformTexture2D(texId, "maintexture", shaderProg, TextureUnit.Texture0);

             _uniformHelper.TryAddUniformTexture2D(normId, "normaltexture", shaderProg, TextureUnit.Texture1);

             _uniformHelper.TryAddUniformTexture2D(metRoughId, "metallicroughness", shaderProg, TextureUnit.Texture2);

             _uniformHelper.TryAddUniformTexture2D(ambientId, "defaultAO", shaderProg, TextureUnit.Texture3);


                //TODO retrieve it properly skybox texture
             _uniformHelper.TryAddUniformTextureCubemap(3, "irradianceMap", shaderProg, TextureUnit.Texture4);

             _uniformHelper.TryAddUniformTextureCubemap(4, "prefilterMap", shaderProg, TextureUnit.Texture5);

             _uniformHelper.TryAddUniformTexture2D(5,"brdfLUT", shaderProg, TextureUnit.Texture6);
        }
    }
}