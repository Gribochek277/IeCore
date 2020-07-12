using IrrationalEngineCore.Core.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrrationalEngineEditor.Interfaces.ViewModels
{
    public interface IObjectBrowserViewModel
    {
        List<ISceneObject> Items { get; set; }
        public event EventHandler UpdateTreeViewHandler;
        public void UpdateTreeView();
    }
}
