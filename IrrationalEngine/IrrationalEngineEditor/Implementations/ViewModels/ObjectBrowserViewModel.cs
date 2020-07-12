using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineEditor.Interfaces.ViewModels;
using System;
using System.Collections.Generic;

namespace IrrationalEngineEditor.Implementations.ViewModels
{
    public class ObjectBrowserViewModel : IObjectBrowserViewModel
    {
        public List<ISceneObject> Items { get ; set; }

        public event EventHandler UpdateTreeViewHandler;

        public void UpdateTreeView()
        {
            UpdateTreeViewHandler?.Invoke(this, null);
        }
    }
}
