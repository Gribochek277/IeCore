using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Entities.Primitives;
using Irrational.Core.SceneObjectComponents;
using Irrational.Core.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Irrational.Core.Renderer.OpenGL.Helpers
{
    public class SkyboxRenderHelper
    {
        public static int RenderCubemapSkybox(Matrix4 view, Matrix4 projection, ISceneObject skybox)
        {            
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);
            SkyboxSceneObjectComponent skyboxComponent = (SkyboxSceneObjectComponent)skybox.components["SkyboxSceneObjectComponent"];
            MeshSceneObjectComponent skyboxMeshComponent = (MeshSceneObjectComponent)skybox.components["MeshSceneObjectComponent"];
            GL.UseProgram(skyboxComponent.Shader.ProgramID);
            skyboxComponent.Shader.EnableVertexAttribArrays();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("projection"), false, ref projection);
            Matrix4 clearTraslationViewMatrix = view.ClearTranslation();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("view"), false, ref clearTraslationViewMatrix);
            new UniformHelper().TryAddUniformTextureCubemap(skyboxComponent.EnvironmentMap, "skybox", skyboxComponent.Shader, TextureUnit.Texture0);           
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.DrawElements(BeginMode.Triangles, skyboxMeshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, 0 * sizeof(uint));            
            GL.DepthFunc(DepthFunction.Less);
            skyboxComponent.Shader.DisableVertexAttribArrays();
            return skyboxMeshComponent.ModelMesh.IndiceCount; 
        }

        public static int RenderHdrToCubemapSkybox(Matrix4 view, Matrix4 projection, ISceneObject skybox)
        {
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);
            SkyboxSceneObjectComponent skyboxComponent = (SkyboxSceneObjectComponent)skybox.components["SkyboxSceneObjectComponent"];
            MeshSceneObjectComponent skyboxMeshComponent = (MeshSceneObjectComponent)skybox.components["MeshSceneObjectComponent"];
            GL.UseProgram(skyboxComponent.Shader.ProgramID);
            skyboxComponent.Shader.EnableVertexAttribArrays();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("projection"), false, ref projection);
            Matrix4 clearTraslationViewMatrix = view.ClearTranslation();
            GL.UniformMatrix4(skyboxComponent.Shader.GetUniform("view"), false, ref clearTraslationViewMatrix);
            bool isSetted =  
                new UniformHelper().
            TryAddUniformTextureCubemap(skyboxComponent.EnvironmentMap, "environmentMap", skyboxComponent.Shader, TextureUnit.Texture0);
            if (!isSetted)
            {
                Console.WriteLine("wrong uniform");
            }
            GL.DrawElements(BeginMode.Triangles, skyboxMeshComponent.ModelMesh.IndiceCount, DrawElementsType.UnsignedInt, 0 * sizeof(uint));
            GL.DepthFunc(DepthFunction.Less);
            skyboxComponent.Shader.DisableVertexAttribArrays();
            return skyboxMeshComponent.ModelMesh.IndiceCount;
        }
    }
}
