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

namespace IrrationalEngineEditor.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ViewModelBase
    {
        public static OpenTKWindow context = new OpenTKWindow();
        //Labels
        public string Rotate => "Rotate something";
        public string Start => "Start the engine window";
        public string Header => "Some text";
        private int rotationValueX = 0;
        private int rotationValueY = 0;
        public int selectedItemIndex { get; set; } = 0;

        public string SelectedItemName { get; set; } = "Nothing was selected yet";

        //Values
        public int DoRotationX
        {
            get { return rotationValueX; }
            set
            {
                if (SceneObjects == null)
                    InitObjects();
                SceneObjects[selectedItemIndex].Rotation = new Vector3(value * 0.001f, SceneObjects[selectedItemIndex].Rotation.Y, SceneObjects[selectedItemIndex].Rotation.Z); rotationValueX = value;
            }
        }

        public int DoRotationY
        {
            get { return rotationValueY; }
            set
            {
                if (SceneObjects == null)
                    InitObjects();
                SceneObjects[selectedItemIndex].Rotation = new Vector3(SceneObjects[selectedItemIndex].Rotation.X, value * 0.001f, SceneObjects[selectedItemIndex].Rotation.Z); rotationValueY = value;
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
            context.Run();
        }

        void SelectItem(int selectedItem)
        {
            Console.WriteLine(selectedItem);
            selectedItemIndex = selectedItem;
            SelectedItemName = SceneObjects[selectedItemIndex].Name;
        }

        void InitObjects()
        {
            SceneManager manager = context.SceneManager as SceneManager;
            Scene scene = manager.Scene as Scene;
            SceneObjects = scene.SceneObjects;
            SelectedItemName = SceneObjects[selectedItemIndex].Name;
        }
    }
}
