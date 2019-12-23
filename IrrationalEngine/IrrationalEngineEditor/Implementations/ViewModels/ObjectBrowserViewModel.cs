using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.Windows;
using IrrationalEngineCore.Core.Windows.Abstractions;
using IrrationalEngineEditor.Interfaces.ViewModels;
using IrrationalEngineEditor.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace IrrationalEngineEditor.Implementations.ViewModels
{
    public class ObjectBrowserViewModel : IObjectBrowserViewModel
    {
        SceneModel sceneModel = new SceneModel();

        public ObjectBrowserViewModel()
        {
            sceneModel = new SceneModel();
            OpenGLWindow window = SceneModel.endgineServiceProvier.GetService<IWindowFactory>().Create() as OpenGLWindow;
            window.Run();
            Items = sceneModel.GetSceneObjects();
        }

        public List<ISceneObject> Items { get ; set; }
    }
}
