using Irrational.Core.CoreManager;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.Windows;
using OpenTK;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace IrrationalEngineEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static OpenTKWindow context = new OpenTKWindow();
        //Labels
        public string Rotate => "Rotate something";
        public string Start => "Start the engine window";
        public string Header => "Some text";
        private int rotationValue = 0;

        public string SelectedItemName { get; set; } = "Nothing was selected yet";

        //Values
        public int DoRotationX
        {
            get { return rotationValue; }
            set
            {
                if (SceneObjects == null)
                    InitObjects();
                SceneObjects[2].Rotation = new Vector3(value * 0.001f, SceneObjects[2].Rotation.Y, SceneObjects[2].Rotation.Z); rotationValue = value;
            }
        }

        public int DoRotationY
        {
            get { return rotationValue; }
            set
            {
                if (SceneObjects == null)
                    InitObjects();
                SceneObjects[2].Rotation = new Vector3(SceneObjects[2].Rotation.X, value * 0.001f, SceneObjects[2].Rotation.Z); rotationValue = value;
            }
        }

        public IList<ISceneObject> SceneObjects { get; private set; }

        //Commands 
        public ReactiveCommand<Unit, Unit> DoRunIrrationalInstance { get; }

        public MainWindowViewModel()
        {
            DoRunIrrationalInstance = ReactiveCommand.Create(RunIrrationalInstance);
        }

        void RunIrrationalInstance()
        {
            context.Run();
        }

        void InitObjects()
        {
            SceneManager manager = context.SceneManager as SceneManager;
            Scene scene = manager.Scene as Scene;
            SceneObjects = scene.SceneObjects;
        }
    }
}
