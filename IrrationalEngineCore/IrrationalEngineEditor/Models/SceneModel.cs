using Irrational.Core.CoreManager;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Windows;
using OpenTK;
using System.Collections.Generic;

namespace IrrationalEngineEditor.Models
{
    public class SceneModel
    {
        public static OpenTKWindow context  = null;

        public static int SelectedItemIndex { get; set; } = 0;

        private IList<ISceneObject> _sceneObjects;

        private float _rotationX = 0;
        private float _rotationY = 0;
        private float _rotationZ = 0;
        private float _positionX = 0;
        private float _positionY = 0;
        private float _positionZ = 0;

        private float _scaleX = 0;
        private float _scaleY = 0;
        private float _scaleZ = 0;

        public float RotationX { get { return _rotationX; }  set { _rotationX = value; Rotate(); } }
        public float RotationY { get { return _rotationY; } set { _rotationY = value; Rotate(); } }
        public float RotationZ { get { return _rotationZ; } set { _rotationZ = value; Rotate(); } }

        public float PositionX { get { return _positionX; } set { _positionX = value; UpdatePosition(); } }
        public float PositionY { get { return _positionY; } set { _positionY = value; UpdatePosition(); } }
        public float PositionZ { get { return _positionZ; } set { _positionZ = value; UpdatePosition(); } }

        public float ScaleX { get { return _scaleX; } set { _scaleX = value; Scale(); } }
        public float ScaleY { get { return _scaleY; } set { _scaleY = value; Scale(); } }
        public float ScaleZ { get { return _scaleZ; } set { _scaleZ = value; Scale(); } }

        public SceneModel()
        {
            if (context == null)
            {
                context = new OpenTKWindow(800, 600, 401, 0);
            }
        }

        public void Rotate()
        {
            if (_sceneObjects != null)
            {
                _sceneObjects[SelectedItemIndex].Rotation = new Vector3(_rotationX, _rotationY, _rotationZ);
            }
        }

        public void UpdatePosition()
        {
            if (_sceneObjects != null)
            {
                _sceneObjects[SelectedItemIndex].Position = new Vector3(_positionX, _positionY, _positionZ);
            }
        }

        public void Scale()
        {
            if (_sceneObjects != null)
            {
                _sceneObjects[SelectedItemIndex].Scale = new Vector3(_scaleX, _scaleY, _scaleZ);
            }
        }

        public IList<ISceneObject> GetSceneObjects()
        {
            if(context != null && _sceneObjects == null)
            { 
                SceneManager manager = context.SceneManager as SceneManager;
                Scene scene = manager.Scene as Scene;
                _sceneObjects = scene.SceneObjects;
                return _sceneObjects;
            }

            return _sceneObjects;
        }
    }
}
