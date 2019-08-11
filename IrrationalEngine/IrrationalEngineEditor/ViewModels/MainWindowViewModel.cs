using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using IrrationalEngineEditor.Models;
using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace IrrationalEngineEditor.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ViewModelBase
    {
        private SceneModel SceneModel { get; set; }
        public string SceneObjectPanel => "Scene objects:";
        public string SceneObjectComponentsPanel => "Scene object components:";
        public int SelectedItemIndex { get { return SceneModel.SelectedItemIndex; } set { SceneModel.SelectedItemIndex = value; } }
        public string Rotation => "Rotation";
        public string Position => "Position";
        public string Scale => "Scale";

        public string SelectedItemName { get { return SelectedSceneObject != null ? SelectedSceneObject.Name : null; } }

        

        public float RotationX { get { return SelectedSceneObject.Rotation.X; } set { SceneModel.RotationX = value; } }
        public float RotationY { get { return SelectedSceneObject.Rotation.Y; } set { SceneModel.RotationY = value; } }
        public float RotationZ { get { return SelectedSceneObject.Rotation.Z; } set { SceneModel.RotationZ = value; } }
        public float PositionX { get { return SelectedSceneObject.Position.X; } set { SceneModel.PositionX = value; } }
        public float PositionY { get { return SelectedSceneObject.Position.Y; } set { SceneModel.PositionY = value; } }
        public float PositionZ { get { return SelectedSceneObject.Position.Z; } set { SceneModel.PositionZ = value; } }
        public float ScaleX { get { return SelectedSceneObject.Scale.X; } set { SceneModel.ScaleX = value; } }
        public float ScaleY { get { return SelectedSceneObject.Scale.Y; } set { SceneModel.ScaleY = value; } }
        public float ScaleZ { get { return SelectedSceneObject.Scale.Z; } set { SceneModel.ScaleZ = value; } }

        public IList<ISceneObject> SceneObjects { get; set; }

        public ISceneObject SelectedSceneObject {
            get {
                if (SceneObjects != null)
                    return SceneObjects[SelectedItemIndex];
                else return new SceneObject();
            }
        }

        public MainWindowViewModel()
        {
            SceneModel = new SceneModel();
            SceneModel.context.LoadingComplete += InitControls;
        }

        void RunIrrationalInstance()
        {
            SceneModel.context.Run();
        }

        private void InitControls(object o, EventArgs e)
        {
            SceneObjects = SceneModel.GetSceneObjects();
        }
    }
}
