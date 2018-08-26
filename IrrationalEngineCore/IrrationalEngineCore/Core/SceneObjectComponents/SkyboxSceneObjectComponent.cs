using Irrational.Core.Entities.Abstractions;
using System;
using Irrational.Core.Shaders;
using Irrational.Core.SceneObjectComponents.SkyboxHelpers;

namespace Irrational.Core.SceneObjectComponents
{
    public class SkyboxSceneObjectComponent : ISceneObjectComponent
    {
        public enum SkyboxType { cubemap, hdr };
        private string _skyboxLocation;
        private int _environmentMapId = -1;
        private int _irradianceMapId = -1;
        private int _prefilteredMapId = -1;
        private int _brdfMapId = -1;
        private ShaderProg _shader;
        private SkyboxType skyboxType = SkyboxType.cubemap;

        public string SkyboxLocation { get { return _skyboxLocation; } }
        public int EnvironmentMap { get { return _environmentMapId; } }
        public int IrradianceMap { get { return _irradianceMapId; } }
        public int PrefilteredMap { get { return _prefilteredMapId; }}
        public int BrdfMap { get { return _brdfMapId; }}
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
                        _environmentMapId = new CubemapLoader().LoadCubemap(_skyboxLocation);
                        break;
                    }
                case SkyboxType.hdr:
                    {
                        HdrLoader loader = new HdrLoader();
                        int[] result = loader.LoadHdr(_skyboxLocation);
                        _environmentMapId = result[1];
                        _irradianceMapId = result[2];
                        _prefilteredMapId = result[3];
                        _brdfMapId = result[4];
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
