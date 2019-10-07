using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IrrationalEngineEditor.Avalonia.Templates
{
    public class SceneObjectComponents : UserControl
    {
        public SceneObjectComponents()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
