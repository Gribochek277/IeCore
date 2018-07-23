﻿using Irrational.Core.CoreManager.Abstractions;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Renderer.Abstractions;
using Irrational.Logic.Scenes;
using System.Collections.Generic;

namespace Irrational.Core.CoreManager
{
    public class SceneManager: ISceneManager
    {
        private IRenderer _renderer;
        private List<ISceneObject> _sceneObjects = new List<ISceneObject>();
        private Scene _scene = new TestScene();
        public SceneManager(IRenderer renderer)
        {
            _renderer = renderer;
        }        

        public void OnLoad()
        {
            _scene.OnLoad();
            foreach (SceneObject sceneObject in _scene.Objects)// TODO: i don't like this part of code
                _sceneObjects.Add(sceneObject);
            _renderer.OnLoad(_sceneObjects);
        }

        public void OnRendered()
        {
            //_scene.OnRendered(); TODO: implement
            _renderer.OnRendered();
        }

        public void OnResized()
        {
           // _scene.OnResized(); TODO: implement
            _renderer.OnResized();
        }

        public void OnUnload()
        {
           // _scene.OnUnload(); TODO: implement
            _renderer.OnUnload();
        }

        public void OnUpdated()
        {
            _scene.OnUpdated();
            _renderer.OnUpdated();
        }
    }
}