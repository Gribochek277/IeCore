using Irrational.Core.Entities.Abstractions;

namespace Irrational.Core.Entities.SceneObjectComponents
{
    class PointLightSceneObjectComponent : ISceneObjectComponent
    {

        public int lightStrenght { get; set; }

        public void OnLoad()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnload()
        {
            throw new System.NotImplementedException();
        }
    }
}
