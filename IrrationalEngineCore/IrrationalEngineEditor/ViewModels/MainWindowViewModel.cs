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

        //Commands 
        public ReactiveCommand<Unit, Unit> DoRunIrrationalInstance { get; }
        public ReactiveCommand<Unit, Unit> DoRotation { get; }

        public MainWindowViewModel()
        {
            DoRunIrrationalInstance = ReactiveCommand.Create(RunIrrationalInstance);
            DoRotation = ReactiveCommand.Create(RotateSomething);
        }       

        void RunIrrationalInstance()
        {
            Program.context.Run();
        }

        void RotateSomething()
        {
            SceneManager manager = Program.context.SceneManager as SceneManager;
            Scene scene = manager.Scene as Scene;
            List<ISceneObject> sceneObjects = scene.SceneObjects;
            sceneObjects[2].Rotation += new Vector3(30, 30, 0);
        }
    }
}
