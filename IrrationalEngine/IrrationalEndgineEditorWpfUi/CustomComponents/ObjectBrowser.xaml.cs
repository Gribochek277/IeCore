using IrrationalEngineCore.Core.Entities.Abstractions;
using IrrationalEngineEditor.Implementations.ViewModels;
using IrrationalEngineEditor.Interfaces.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows.Controls;

namespace IrrationalEndgineEditorWpfUi.CustomComponents
{
    /// <summary>
    /// Interaction logic for ObjectBrowser.xaml
    /// </summary>
    public partial class ObjectBrowser : UserControl
    {
        private readonly IObjectBrowserViewModel _objectBrowserViewModel;
        public ObjectBrowser()
        {
            InitializeComponent();
           
            _objectBrowserViewModel = App.ServiceProvider.GetRequiredService<ObjectBrowserViewModel>();
            
        }

        private void TreeView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TreeView tree = sender as TreeView;
            foreach (ISceneObject sceneObject in _objectBrowserViewModel.Items)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = sceneObject.Name;
                item.ItemsSource = sceneObject.components.Select(c=>c.Key).ToList();
                tree.Items.Add(item);
            }
        }
    }
}
