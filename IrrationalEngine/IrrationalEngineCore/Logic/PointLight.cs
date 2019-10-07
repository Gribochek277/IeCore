using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using OpenTK;

namespace IrrationalEngineCore.Logic
{
    public class PointLight : SceneObject
    {
        public PointLight(Vector3 color, float lightStr): base("PointLight")
        {
            AddComponent(new PointLightSceneObjectComponent()
                { LightStrenght = lightStr, Color = color
            });
        }
    }
}
