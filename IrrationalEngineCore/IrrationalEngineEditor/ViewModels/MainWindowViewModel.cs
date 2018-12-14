using Irrational.Core.CoreManager;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Windows;
using OpenTK;
using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace IrrationalEngineEditor.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ViewModelBase
    {
        public static OpenTKWindow context = new OpenTKWindow();
        //Labels
        public string Rotate => "Rotate something";
        public string Start => "Load scene";
        public string Header => "Some text";
        private int rotationValueX = 0;
        private int rotationValueY = 0;
        private int rotationValueZ = 0;
        private int transfromValueX = 0;
        private int transfromValueY = 0;
        private int transfromValueZ = 0;
        public int selectedItemIndex { get; set; } = 0;

        public string SelectedItemName { get; set; } = "Nothing was selected yet";

        //Values
        public int DoRotationX
        {
            get { return rotationValueX; }
            set
            {
                  SceneObjects[selectedItemIndex].Rotation = new Vector3(value * 0.001f, SceneObjects[selectedItemIndex].Rotation.Y, SceneObjects[selectedItemIndex].Rotation.Z); rotationValueX = value;
            }
        }        

        public int DoRotationY
        {
            get { return rotationValueY; }
            set
            {
                 SceneObjects[selectedItemIndex].Rotation = new Vector3(SceneObjects[selectedItemIndex].Rotation.X, value * 0.001f, SceneObjects[selectedItemIndex].Rotation.Z); rotationValueY = value;
            }
        }

        public int DoRotationZ
        {
            get { return rotationValueZ; }
            set
            {
                 SceneObjects[selectedItemIndex].Rotation = new Vector3(SceneObjects[selectedItemIndex].Rotation.X, SceneObjects[selectedItemIndex].Rotation.Y, value * 0.001f); rotationValueZ = value;
            }
        }

        public int DoTransformX
        {
            get { return transfromValueX; }
            set
            {
                SceneObjects[selectedItemIndex].Position = new Vector3(value * 0.001f, SceneObjects[selectedItemIndex].Position.Y, SceneObjects[selectedItemIndex].Position.Z); transfromValueX = value;
            }
        }

        public int DoTransformY
        {
            get { return transfromValueY; }
            set
            {
                 SceneObjects[selectedItemIndex].Position = new Vector3(SceneObjects[selectedItemIndex].Position.X, value * 0.001f, SceneObjects[selectedItemIndex].Position.Z); transfromValueY = value;
            }
        }

        public int DoTransformZ
        {
            get { return transfromValueZ; }
            set
            {
                SceneObjects[selectedItemIndex].Position = new Vector3(SceneObjects[selectedItemIndex].Position.X, SceneObjects[selectedItemIndex].Position.Y, value * 0.001f); transfromValueZ = value;
            }
        }

        public IList<ISceneObject> SceneObjects { get; private set; }

        //Commands 
        public ReactiveCommand<Unit, Unit> DoRunIrrationalInstance { get; }
        public ReactiveCommand<int, Unit> DoSelectItem { get; }

        public MainWindowViewModel()
        {
            DoRunIrrationalInstance = ReactiveCommand.Create(RunIrrationalInstance);
            DoSelectItem = ReactiveCommand.Create<int>(SelectItem);
        }

        void RunIrrationalInstance()
        {
           context.LoadingComplete += InitObjects;
           context.Run();
        }

        void SelectItem(int selectedItem)
        {
            Console.WriteLine(selectedItem);
            selectedItemIndex = selectedItem;
            SelectedItemName = SceneObjects[selectedItemIndex].Name;
        }

        void InitObjects(object sender, EventArgs e)
        {
            //Think about callback
                    SceneManager manager = context.SceneManager as SceneManager;
                    Scene scene = manager.Scene as Scene;
                    SceneObjects = scene.SceneObjects;
                    SelectedItemName = SceneObjects[selectedItemIndex].Name;
        }
    }
}
