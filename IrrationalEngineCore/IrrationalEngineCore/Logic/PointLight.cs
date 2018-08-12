using Irrational.Core.Entities;
using Irrational.Core.SceneObjectComponents;
using OpenTK;

namespace Irrational.Logic
{
    public class PointLight : SceneObject
    {
        public PointLight(Vector3 color, float lightStr)
        {
            AddComponent(new PointLightSceneObjectComponent()
                { LightStrenght = lightStr, Color = color
            });
        }
    }
}
