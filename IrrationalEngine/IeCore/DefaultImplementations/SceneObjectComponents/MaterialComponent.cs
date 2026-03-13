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

		private readonly bool _useAnimatedShader;

		public string Name => "MaterialSceneObjectComponent";

		public MaterialComponent(IShaderProgram shaderProgram, bool useAnimatedShader = false)
		{
			ShaderProgram = shaderProgram;
			_useAnimatedShader = useAnimatedShader;
		}

		public void OnLoad()
		{
			if (_useAnimatedShader)
			{
				ShaderProgram.LoadShaderFromString(DefaultDiffuseShaderAnimated.VertexShader, VertexShaderName, ShaderType.VertexShader);
				ShaderProgram.LoadShaderFromString(DefaultDiffuseShaderAnimated.FragmentShader, FragmentShaderName, ShaderType.FragmentShader);
			}
			else
			{
				ShaderProgram.LoadShaderFromString(DefaultDiffuseShader.VertexShader, VertexShaderName, ShaderType.VertexShader);
				ShaderProgram.LoadShaderFromString(DefaultDiffuseShader.FragmentShader, FragmentShaderName, ShaderType.FragmentShader);
			}
			ShaderProgram.LinkShadersToProgram();
			ShaderProgram.GenBuffers();
		}

		public void OnUnload()
		{
			ShaderProgram.Dispose();
		}
	}
}
