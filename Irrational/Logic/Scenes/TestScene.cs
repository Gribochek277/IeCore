namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        public override void OnLoad()
        {
            base.OnLoad();

            // Lion gameObject = new Lion();
            //Knight knight = new Knight();
            GLtf2Helm gltf2helm = new GLtf2Helm();
            _sceneObjects.Add(gltf2helm);
            // _sceneObjects.Add(knight);
            // _sceneObjects.Add(gameObject);
        }
    }
}
