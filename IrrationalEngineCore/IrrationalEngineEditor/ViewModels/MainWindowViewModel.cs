using Irrational.Core.CoreManager;
using Irrational.Core.Entities;
using Irrational.Core.Entities.Abstractions;
using Irrational.Core.SceneObjectComponents;
using OpenTK;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace IrrationalEngineEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Labels
        public string Rotate => "Rotate something";
        public string Start => "Start the engine window";
        public string SelectedItemName => "No selected item on the scene";
        private int rotationValue = 0;

        //Values
        public int DoRotationX
        {
            get { return rotationValue; }
            set
            {
                if (sceneObjects == null)
                    InitObjects();
                sceneObjects[2].Rotation = new Vector3(value * 0.001f, sceneObjects[2].Rotation.Y, sceneObjects[2].Rotation.Z); rotationValue = value;
            }
        }

        public int DoRotationY
        {
            get { return rotationValue; }
            set
            {
                if (sceneObjects == null)
                    InitObjects();
                sceneObjects[2].Rotation = new Vector3(sceneObjects[2].Rotation.X, value * 0.001f, sceneObjects[2].Rotation.Z); rotationValue = value;
            }
        }


        private List<ISceneObject> sceneObjects;

        //Commands 
        public ReactiveCommand<Unit, Unit> DoRunIrrationalInstance { get; }

        public MainWindowViewModel()
        {
            DoRunIrrationalInstance = ReactiveCommand.Create(RunIrrationalInstance);
        }

        void RunIrrationalInstance()
        {
            Program.context.Run();
        }

        void InitObjects()
        {
            SceneManager manager = Program.context.SceneManager as SceneManager;
            Scene scene = manager.Scene as Scene;
            sceneObjects = scene.SceneObjects;
        }
    }
}
