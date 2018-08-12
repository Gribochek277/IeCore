using Irrational.Core.Entities.Abstractions;
using OpenTK;

namespace Irrational.Core.SceneObjectComponents
{
    class PointLightSceneObjectComponent : ISceneObjectComponent
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
