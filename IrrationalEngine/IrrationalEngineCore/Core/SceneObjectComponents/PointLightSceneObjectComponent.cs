using IrrationalEngineCore.Core.Entities.Abstractions;
using OpenTK;

namespace IrrationalEngineCore.Core.SceneObjectComponents
{
    public class PointLightSceneObjectComponent : ISceneObjectComponent
    {
        public float LightStrenght { get; set; }
        public Vector3 Color { get; set; }

        public void OnLoad()
        {
           
        }

        public void OnUnload()
        {
            
        }
    }
}
