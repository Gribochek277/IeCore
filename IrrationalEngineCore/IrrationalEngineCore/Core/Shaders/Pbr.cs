using System;
using System.Collections.Generic;
using System.Linq;
using Irrational.Core.Entities;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Core.Renderer.OpenGL.Helpers;
using Irrational.Core.SceneObjectComponents;
using Irrational.Core.Shaders;
using IrrationalEngineCore.Core.Shaders.Abstractions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace IrrationalEngineCore.Core.Shaders
{
    public class Pbr : IShaderImplementation
    {
        public ShaderProg shaderProg {get;}

        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        private Dictionary<string, int> _textures = new Dictionary<string, int>();      

        public float randomCoeff = 0;

        public Dictionary<string, Material> Materials { get => _materials; set => _materials = value; }
        public Dictionary<string, int> Textures { get => _textures; set => _textures = value; }

        private UniformHelper _uniformHelper;

        private const string maintexture = "maintexture";
        private const string normaltexture = "normaltexture";
        private const string metallicRoughness = "metallicroughness";
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
            int texId = Textures[Materials.FirstOrDefault().Value.DiffuseMap];
            int normId = Textures[Materials.FirstOrDefault().Value.NormalMap];
            int metRoughId = Textures[Materials.FirstOrDefault().Value.MetallicRoughness];
            int ambientId = Textures[Materials.FirstOrDefault().Value.AmbientMap];

             _uniformHelper.TryAddUniformTexture2D(texId, maintexture, shaderProg, TextureUnit.Texture0);

             _uniformHelper.TryAddUniformTexture2D(normId, normaltexture, shaderProg, TextureUnit.Texture1);

             _uniformHelper.TryAddUniformTexture2D(metRoughId, metallicRoughness, shaderProg, TextureUnit.Texture2);

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