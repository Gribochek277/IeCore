using System;
using System.IO;
using Irrational.Core.Shaders;
using OpenTK;

namespace Irrational.Logic.Scenes
{
    public class TestLinuxScene : Scene
    {
        public override void OnLoad()
        {
            ShaderProg skyboxShader = 
                new ShaderProg("vs_cubemap.glsl", "fs_cubemap.glsl");
            base._skybox = new Skybox(skyboxShader,$"Resources{Path.DirectorySeparatorChar}Cubemaps");
            base.OnLoad();
            PointLight light1 = new PointLight(new Vector3(1f, 1, 1f), 1);
            light1.Position = new Vector3(-2f, 0, 0);
            _sceneObjects.Add(light1);
            PointLight light2 = new PointLight(new Vector3(1f, 1, 1f), 2);
            light2.Position = new Vector3(0, 2, 0);
            _sceneObjects.Add(light2);
            //PointLight light3 = new PointLight(new Vector3(1f, 1, 1f), 3);
            //light3.Position = new Vector3(2, 0, 0);
            //_sceneObjects.Add(light3);
            //PointLight light4 = new PointLight(new Vector3(1f, 1, 1f), 3);
            //light4.Position = new Vector3(1, 1, -1);
            //_sceneObjects.Add(light4);



            //Lion gameObject = new Lion();
            //_sceneObjects.Add(gameObject);
            Knight knight = new Knight();
            _sceneObjects.Add(knight);
            //GLtf2Helm gltf2helm = new GLtf2Helm();
            //_sceneObjects.Add(gltf2helm);

        }

        public override void OnUpdated()
        {
           
            base.OnUpdated();
        }
    }
}
