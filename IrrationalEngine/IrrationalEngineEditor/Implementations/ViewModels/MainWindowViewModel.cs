using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineCore.Core.Windows;
using IrrationalEngineCore.Core.Windows.Abstractions;
using IrrationalEngineEditor.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using IrrationalEngineEditor.Interfaces.ViewModels;

namespace IrrationalEngineEditor.Implementations.ViewModels
{
    public class MainWindowViewModel: IMainWindowViewModel
    {
        private SceneModel _sceneModel;

        private readonly OpenGLWindow window;

        private readonly IObjectBrowserViewModel _objectBrowserViewModel;

        private List<ISceneObject> _sceneObjects { get; set; }

        public MainWindowViewModel(SceneModel sceneModel, IObjectBrowserViewModel objectBrowserViewModel)
        {
            _sceneModel = sceneModel;
            _objectBrowserViewModel = objectBrowserViewModel;
            window = SceneModel.endgineServiceProvider.GetService<IWindowFactory>().Create() as OpenGLWindow;
            window.LoadingComplete += InitControls;

        }

        public void NewWindow()
        {
            window.Run();
        }

        private void InitControls(object o, EventArgs e)
        {
            _sceneObjects = _sceneModel.SceneObjects;
            _objectBrowserViewModel.Items = _sceneObjects;
            _objectBrowserViewModel.UpdateTreeView();
        }
    }
}
