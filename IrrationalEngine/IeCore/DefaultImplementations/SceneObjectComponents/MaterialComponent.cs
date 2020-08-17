﻿using IeCore.DefaultImplementations.Shaders;
using IeCoreEntites.Materials;
using IeCoreEntites.Shaders;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreInterfaces.Shaders;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
    public class MaterialComponent : IMaterialComponent
    {
        const string FragmentShaderName = "DefaultFragmentShader";
        const string VertexShaderName = "DefaultVertexShader";
        public Dictionary<string, Material> materials { get; } = new Dictionary<string, Material>();

        public IShaderProgram ShaderProgram { get; } = IrrationalEngine.ServiceProvider.GetService<IShaderProgram>();

        public string Name => "MaterialSceneObjectComponent";


        public void OnLoad()
        {
           ShaderProgram.LoadShaderFromString(DefaultDiffuseShader.VertexShader, VertexShaderName, ShaderType.VertexShader);
           ShaderProgram.LoadShaderFromString(DefaultDiffuseShader.FragmentShader, FragmentShaderName, ShaderType.FragmentShader);
           ShaderProgram.LinkShadersToProgram();
          
        }

        public void OnUnload()
        {
            ShaderProgram.Dispose();
        }
    }
}
