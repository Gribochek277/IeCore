using Irrational.Core.Entities.Abstractions;
using System;
using System.IO;
using Irrational.Core.Shaders;
using Irrational.Core.Entities.SceneObjectComponents.SkyboxHelpers;

namespace Irrational.Core.Entities.SceneObjectComponents
{
    public class SkyboxSceneObjectComponent : ISceneObjectComponent
    {
        public enum SkyboxType { cubemap, hdr };
        private string _skyboxLocation;
        private int _texId = -1;
        private ShaderProg _shader;
        private SkyboxType skyboxType = SkyboxType.cubemap;

        public string SkyboxLocation { get { return _skyboxLocation; } }
        public int TexId { get { return _texId; } }
        public ShaderProg Shader { get { return _shader ?? null; } }

        public SkyboxSceneObjectComponent(string location, ShaderProg Shader, SkyboxType type)
        {
            _skyboxLocation = location;
            _shader = Shader;
            skyboxType = type;
        }

        public SkyboxSceneObjectComponent(string location, SkyboxType type)
        {
            _skyboxLocation = location;
            _shader = Shader;
            skyboxType = type;
            switch (skyboxType)
            {
                case SkyboxType.cubemap:
                    {
                        _shader = new ShaderProg("vs_cubemap.glsl", "fs_cubemap.glsl", true);
                        break;
                    }
                case SkyboxType.hdr:
                    {
                        _shader = new ShaderProg("vs_hdr.glsl", "fs_hdr.glsl", true);
                        break;
                    }
                default: break;
            }
        }
        

        public void OnLoad()
        {
            switch (skyboxType) {
                case SkyboxType.cubemap:
                    {
                        _texId = new CubemapLoader().LoadCubemap(_skyboxLocation);
                        break;
                    }
                case SkyboxType.hdr:
                    {
                        HdrLoader loader = new HdrLoader();
                        _texId = loader.LoadHdr(_skyboxLocation);
                        break;
                    }
                default: break;
            }
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
