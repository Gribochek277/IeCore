using IrrationalEngineCore;
using Microsoft.Extensions.DependencyInjection;
using IrrationalEngineCore.Core.CoreManager.Abstractions;
using IrrationalEngineCore.Core.Entities;
using IrrationalEngineCore.Core.Entities.Abstractions;
using System;
using System.Collections.Generic;
using IrrationalEngineCore.Core.CoreManager;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IrrationalEngineEditor.Models
{
    public class SceneModel: INotifyPropertyChanged
    {
        public static IServiceProvider endgineServiceProvider  = null;

        public static int SelectedItemIndex { get; set; } = 0;

        List<ISceneObject> sceneObjects = new List<ISceneObject>();
        public List<ISceneObject> SceneObjects { get { return sceneObjects; } 
            set
            {
                sceneObjects = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        public SceneModel()
        {
            if (endgineServiceProvider == null)
            {
               
                endgineServiceProvider = IrrationalEngine.ServiceProvider;

                ISceneManager manager = endgineServiceProvider.GetService<ISceneManager>() as SceneManager;
                Scene scene = manager.Scene as Scene;
                SceneObjects = scene.SceneObjects;
            }
        }
    }
}
