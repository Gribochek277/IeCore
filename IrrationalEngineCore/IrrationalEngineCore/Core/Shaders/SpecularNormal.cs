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
    public class SpecularNormal : IShaderImplementation
    {
        public ShaderProg shaderProg {get;}

        public Dictionary<string, Material> Materials { get => _materials; set => _materials = value; }
        public Dictionary<string, int> Textures { get => _textures; set => _textures = value; }

        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        private Dictionary<string, int> _textures = new Dictionary<string, int>();  
        private UniformHelper _uniformHelper;

        private const string maintexture = "maintexture";
        private const string normaltexture = "normaltexture";
        private const string amient = "ambientStr";
        private const string specular = "specStr";
        private const string lightColor = "lightColor[0]";
        private const string lightPosition = "lightPos[0]";
        private const string cameraPosition = "cameraPosition";

        public SpecularNormal()
        {
            shaderProg = new ShaderProg("vs_norm.glsl", "fs_norm.glsl", true);
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
            int texId = Textures[Materials.FirstOrDefault().Value.DiffuseMap];
            int normId = Textures[Materials.FirstOrDefault().Value.NormalMap];

             _uniformHelper.TryAddUniformTexture2D(texId, maintexture, shaderProg, TextureUnit.Texture0);

             _uniformHelper.TryAddUniformTexture2D(normId, normaltexture, shaderProg, TextureUnit.Texture1);

             _uniformHelper.TryAddUniform1(1f, amient, shaderProg);

             _uniformHelper.TryAddUniform1(1f, specular, shaderProg);

              Vector3[] lightpositions = new Vector3[pipelineData.Lights.Count()];
              Vector3[] lightcolors = new Vector3[pipelineData.Lights.Count()];
                
                    var lightComponent =  (PointLightSceneObjectComponent)pipelineData.Lights[0].components["PointLightSceneObjectComponent"];
                    lightcolors[0] = lightComponent.Color * lightComponent.LightStrenght;
                    lightpositions[0] = pipelineData.Lights[0].Position;
                

                bool suc = _uniformHelper.TryAddUniform1(lightcolors, lightColor, shaderProg);
                bool suc2 = _uniformHelper.TryAddUniform1(lightpositions, lightPosition, shaderProg);

            _uniformHelper.TryAddUniform1(pipelineData.Cam.Position, cameraPosition, shaderProg);
      }
    }
}