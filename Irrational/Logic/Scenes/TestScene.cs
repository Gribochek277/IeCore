using Irrational.Core.Entities.SceneObjectComponents;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        private int pickedObject = 2;
        bool flag = true;
        public override void OnLoad()
        {
            base.OnLoad();            
            PointLight light1 = new PointLight(new Vector3(0.3f, 0.1f, 0.1f), 3);
            light1.Position = new Vector3(-1f, 0, 0);
            //PointLight light2 = new PointLight(new Vector3(0, 0, 1),2);
            //light2.Position = new Vector3(1f, 0,0);
            //PointLight light3 = new PointLight(new Vector3(0, 0, .3f), 10);
            //light3.Position = new Vector3(1, 0, 0);
            //PointLight light4 = new PointLight(new Vector3(.3f, 0, 0), 10);
            //light4.Position = new Vector3(0, 0, -1);
            _sceneObjects.Add(light1);
            //_sceneObjects.Add(light2);
            //_sceneObjects.Add(light3);
            //_sceneObjects.Add(light4);
            Lion gameObject = new Lion();
            _sceneObjects.Add(gameObject);
            //Knight knight = new Knight();
            //GLtf2Helm gltf2helm = new GLtf2Helm();
            //_sceneObjects.Add(gltf2helm);
            //_sceneObjects.Add(knight);
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

