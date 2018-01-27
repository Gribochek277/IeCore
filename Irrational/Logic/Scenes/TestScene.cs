namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        public override void OnLoad()
        {
            base.OnLoad();

            Lion gameObject = new Lion();
            Knight knight = new Knight();
            _sceneObjects.Add(knight);
            _sceneObjects.Add(gameObject);
        }
    }
}
