using System;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.SceneObjectComponents;
using IrrationalEngineCore.Core.Shaders;
using OpenTK;
using OpenTK.Input;

namespace IrrationalEngineCore.Logic.Scenes
{
    public class TestLinuxScene : Scene
    {

        int pickedObject = 2;
        bool flag = true;
        public override void OnLoad()
        {
            ShaderProg skyboxShader = 
                new ShaderProg("vs_cubemap.glsl", "fs_cubemap.glsl", true);
            _skybox = new Skybox(skyboxShader);
            base.OnLoad();
            //Lion gameObject = new Lion();
            //_sceneObjects.Add(gameObject);
            Knight knight = new Knight();
            _sceneObjects.Add(knight);
             PointLight light1 = new PointLight(new Vector3(0f, 0f, 0f), 1);
            light1.Position = new Vector3(0, 0, -1);
            _sceneObjects.Add(light1);
           
            //GLtf2Helm gltf2helm = new GLtf2Helm();
            //_sceneObjects.Add(gltf2helm);

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
