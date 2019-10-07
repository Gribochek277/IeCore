using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.Windows;
using IrrationalEngineCore.Core.Windows.Abstractions;
using IrrationalEngineEditor.Models;
using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using Microsoft.Extensions.DependencyInjection;

namespace IrrationalEngineEditor.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SceneObjectComponentsViewModel: ViewModelBase
    {
        private SceneModel SceneModel { get; set; }
        private readonly OpenGLWindow window;
        public int SelectedItemIndex { get { return SceneModel.SelectedItemIndex; } set { SceneModel.SelectedItemIndex = value; } }
        public string SelectedComponent { get
            {
                if (SceneObjects != null)
                    return SceneObjects[SelectedItemIndex].components["MeshSceneObjectComponent"].GetType().ToString();
                else return "dummy";
            }
        }

        public string SelectedItemName { get; set; }



        public ReactiveCommand<string, Unit> ChangeListItemCommand { get; }

        public SceneObjectComponentsViewModel()
        {
            SceneModel = new SceneModel();
            window = SceneModel.endgineServiceProvier.GetService<IWindowFactory>().Create() as OpenGLWindow;
            window.LoadingComplete += InitControls;
            ChangeListItemCommand = ReactiveCommand.Create<string>(ChangeListItem);
        }

        public IList<ISceneObject> SceneObjects { get; set; }

        public ISceneObject SelectedSceneObject
        {
            get
            {
                if (SceneObjects != null)
                    return SceneObjects[SelectedItemIndex];
                else return new SceneObject();
            }
        }
       
        public void ChangeListItem(string parameter)
        {
                SelectedItemName = parameter;
        }

        private void InitControls(object o, EventArgs e)
        {
            SceneObjects = SceneModel.GetSceneObjects();
        }
    }
}
