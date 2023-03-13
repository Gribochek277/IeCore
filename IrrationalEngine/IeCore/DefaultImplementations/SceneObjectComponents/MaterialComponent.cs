using IeCore.DefaultImplementations.Shaders;
using IeCoreEntities.Materials;
using IeCoreEntities.Shaders;
using IeCoreInterfaces.SceneObjectComponents;
using IeCoreInterfaces.Shaders;
using System.Collections.Generic;

namespace IeCore.DefaultImplementations.SceneObjectComponents
{
	public class MaterialComponent : IMaterialComponent
	{
		private const string FragmentShaderName = "DefaultFragmentShader";
		private const string VertexShaderName = "DefaultVertexShader";
		public Dictionary<string, Material> Materials { get; } = new Dictionary<string, Material>();

		public IShaderProgram ShaderProgram { get; }


		public string Name => "MaterialSceneObjectComponent";

		public MaterialComponent(IShaderProgram shaderProgram)
		{
			ShaderProgram = shaderProgram;
		}

		public void OnLoad()
		{
			//TODO: implement shader variation possibility.
			//TODO: encapsulate link and gen buffers.
			ShaderProgram.LoadShaderFromString(DefaultDiffuseShaderAnimated.VertexShader, VertexShaderName, ShaderType.VertexShader);
			ShaderProgram.LoadShaderFromString(DefaultDiffuseShaderAnimated.FragmentShader, FragmentShaderName, ShaderType.FragmentShader);
			ShaderProgram.LinkShadersToProgram();
			ShaderProgram.GenBuffers();
		}

		public void OnUnload()
		{
			ShaderProgram.Dispose();
		}
	}
}
