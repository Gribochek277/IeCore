using Irrational.Core.Entities;
using Irrational.Core.Entities.Primitives;
using Irrational.Core.SceneObjectComponents;
using OpenTK;
using OpenTK.Input;
using System;

namespace Irrational.Logic.Scenes
{
    public class TestScene : Scene
    {
        int pickedObject = 2;
        bool flag = true;
        public override void OnLoad()
        {
            base.OnLoad();
            //  Lion gameObject = new Lion();
            //_sceneObjects.Add(gameObject);
          //  Cerebrus gameObject = new Cerebrus();
          //  _sceneObjects.Add(gameObject);

            
            //Knight knight = new Knight();
            //_sceneObjects.Add(knight);
          // GLtf2Helm gltf2helm = new GLtf2Helm();
          //  _sceneObjects.Add(gltf2helm);

            GLtf2Helm gltf2helm2 = new GLtf2Helm();
            _sceneObjects.Add(gltf2helm2);

            PointLight light1 = new PointLight(new Vector3(300.0f, 300.0f, 300.0f), 1);
            light1.Position = new Vector3(10.0f,  10.0f, 10.0f);
            _sceneObjects.Add(light1);
            //PointLight light2 = new PointLight(new Vector3(300.0f, 300.0f, 300.0f), 1);
            //light2.Position = new Vector3(10.0f,  10.0f, 10.0f);
            //_sceneObjects.Add(light2);
            //PointLight light3 = new PointLight(new Vector3(300.0f, 300.0f, 300.0f), 1);
            //light3.Position = new Vector3(-10.0f, -10.0f, 10.0f);
            //_sceneObjects.Add(light3);
            //PointLight light4 = new PointLight(new Vector3(300.0f, 300.0f, 300.0f), 1);
            //light4.Position = new Vector3(10.0f, -10.0f, 10.0f);
            //_sceneObjects.Add(light4);



          
        }

        public override void OnUpdated()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Tab))
            {                
                if(flag)
                    {
                     pickedObject = pickedObject == 2 ? 3 : 2;
                    }
                    
                    Console.WriteLine("Picked object is " + pickedObject);
                    flag = false;
                               
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

