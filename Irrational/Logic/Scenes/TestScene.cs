using Irrational.Core.Entities.SceneObjectComponents;
using OpenTK.Input;
using System;

namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        private int pickedObject = 0;
        public override void OnLoad()
        {
            base.OnLoad();

           // Lion gameObject = new Lion();
            Knight knight = new Knight();
          //  GLtf2Helm gltf2helm = new GLtf2Helm();
            //_sceneObjects.Add(gltf2helm);
            _sceneObjects.Add(knight);
            //_sceneObjects.Add(gameObject);      
        }

        public override void OnUpdated()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Tab))
            {
                pickedObject = pickedObject < _sceneObjects.Count ? pickedObject+=1 : 0;
                
                Console.WriteLine("Picked object is " + pickedObject);
            }
            try { 
            var manipulation = (BasicManipulationsComponent) _sceneObjects[pickedObject].components["BasicManipulationsComponent"];
                manipulation.OnUpdated();
            }catch
            { }
            base.OnUpdated();
        }
    }
}

