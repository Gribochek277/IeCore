using Irrational.Core.Entities.SceneObjectComponents;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        private int pickedObject = 2;
        private float time = 0;
        bool flag = true;
        public override void OnLoad()
        {
            base.OnLoad();
            PointLight light1 = new PointLight(new Vector3(1f, 1, 1f), 1);
            light1.Position = new Vector3(-2f, 0, 0);
            _sceneObjects.Add(light1);
            PointLight light2 = new PointLight(new Vector3(1f, 1, 1f), 2);
            light2.Position = new Vector3(0, 2, 0);
            _sceneObjects.Add(light2);
            PointLight light3 = new PointLight(new Vector3(1f, 1, 1f), 3);
            light3.Position = new Vector3(2, 0, 0);
            _sceneObjects.Add(light3);
            PointLight light4 = new PointLight(new Vector3(1f, 1, 1f), 3);
            light4.Position = new Vector3(1, 1, -1);
            _sceneObjects.Add(light4);



            Lion gameObject = new Lion();
            _sceneObjects.Add(gameObject);
            //Knight knight = new Knight();
            //_sceneObjects.Add(knight);
            GLtf2Helm gltf2helm = new GLtf2Helm();
            _sceneObjects.Add(gltf2helm);

        }

        public override void OnUpdated()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Tab))
            {                
                if(flag)
                    {
                    pickedObject = pickedObject < _sceneObjects.Count ? pickedObject+=1 : 0;
                    
                    Console.WriteLine("Picked object is " + pickedObject);
                    flag = false;
                }               
            }
            if (Keyboard.GetState().IsKeyUp(Key.Tab))
            {
                flag = true;
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

