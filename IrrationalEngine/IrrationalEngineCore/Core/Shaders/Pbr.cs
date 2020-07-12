using System;
using System.Collections.Generic;
using System.Linq;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Renderer.Abstractions;
using IrrationalEngineCore.Core.Renderer.OpenGL.Helpers;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using IrrationalEngineCore.Core.Shaders.Abstractions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace IrrationalEngineCore.Core.Shaders
{
    public class Pbr : IShaderImplementation
    {
        public ShaderProg shaderProg {get;}

        public float randomCoeff = 0;

        public Dictionary<string, Material> Materials { get; set; } = new Dictionary<string, Material>();
        public Dictionary<string, int> Textures { get; set; } = new Dictionary<string, int>();

        private UniformHelper _uniformHelper;

        private const string maintexture = "maintexture";
        private const string normaltexture = "normaltexture";
        private const string metallicRoughness = "metallicroughness";

        private const string metallic = "metallicSampler";
        private const string roughness = "roughnessSampler";
        private const string amibenOclussionMap = "defaultAO";
        private const string irradianceMap = "irradianceMap";
        private const string prefilterMap = "prefilterMap";
        private const string brdf = "brdfLUT";
        private const string numberOfLights = "numberOfLights";
        private const string lightColor = "lightColor[0]";
        private const string lightPosition = "lightPos[0]";
        private const string cameraPosition = "cameraPosition";

        public Pbr()
        {
            shaderProg =  new ShaderProg("vs_norm.glsl", "fs_PBR_gltf_pipeline_with_maps.glsl", true);            
            _uniformHelper = new UniformHelper();
        }
        public void SetSpecificUniforms(IPipelineData pipelineData)
        {
            int texId = -1;
            int normId = -1; 
            int metRoughId = -1;
            int metallicId = -1;
            int roughnessId = -1;
            int ambientId = -1; 


            Textures.TryGetValue(Materials.FirstOrDefault().Value.DiffuseMap, out texId);
            Textures.TryGetValue(Materials.FirstOrDefault().Value.NormalMap, out normId);
            Textures.TryGetValue(Materials.FirstOrDefault().Value.MetallicRoughness, out metRoughId);
            Textures.TryGetValue(Materials.FirstOrDefault().Value.Metallic, out metallicId);
            Textures.TryGetValue(Materials.FirstOrDefault().Value.Roughness, out roughnessId);
            Textures.TryGetValue(Materials.FirstOrDefault().Value.AmbientMap, out ambientId);

            _uniformHelper.TryAddUniformTexture2D(texId, maintexture, shaderProg, TextureUnit.Texture0);

             _uniformHelper.TryAddUniformTexture2D(normId, normaltexture, shaderProg, TextureUnit.Texture1);

             _uniformHelper.TryAddUniformTexture2D(metRoughId, metallicRoughness, shaderProg, TextureUnit.Texture2);

            _uniformHelper.TryAddUniformTexture2D(metallicId, metallic, shaderProg, TextureUnit.Texture2);
            _uniformHelper.TryAddUniformTexture2D(roughnessId, roughness, shaderProg, TextureUnit.Texture2);

            _uniformHelper.TryAddUniformTexture2D(ambientId, amibenOclussionMap, shaderProg, TextureUnit.Texture3);
            
             _uniformHelper.TryAddUniformTextureCubemap(pipelineData.SkyboxComponent.IrradianceMap, irradianceMap, shaderProg, TextureUnit.Texture4);

             _uniformHelper.TryAddUniformTextureCubemap(pipelineData.SkyboxComponent.PrefilteredMap, prefilterMap, shaderProg, TextureUnit.Texture5);
      
             _uniformHelper.TryAddUniformTexture2D(pipelineData.SkyboxComponent.BrdfMap, brdf, shaderProg, TextureUnit.Texture6);

            _uniformHelper.TryAddUniform1(randomCoeff, "randomCoeff", shaderProg);

              _uniformHelper.TryAddUniform1(pipelineData.Lights.Count(), numberOfLights, shaderProg);
                Vector3[] lightpositions = new Vector3[pipelineData.Lights.Count()];
                Vector3[] lightcolors = new Vector3[pipelineData.Lights.Count()];
                for (int j = 0; j < pipelineData.Lights.Count; ++j) {
                    var lightComponent =  (PointLightSceneObjectComponent)pipelineData.Lights[j].components["PointLightSceneObjectComponent"];
                    lightcolors[j] = lightComponent.Color * lightComponent.LightStrenght;
                    lightpositions[j] = pipelineData.Lights[j].Position;
                }

                bool suc = _uniformHelper.TryAddUniform1(lightcolors, lightColor, shaderProg);
                bool suc2 = _uniformHelper.TryAddUniform1(lightpositions, lightPosition, shaderProg);

                _uniformHelper.TryAddUniform1(pipelineData.Cam.Position, cameraPosition, shaderProg);

                _uniformHelper.TryAddUniform1(Materials.FirstOrDefault().Value.NormalScale, "normalScale", shaderProg);
        }

        public void OnLoad()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }
    }
}