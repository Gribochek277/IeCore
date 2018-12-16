using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IrrationalEngineEditor.Avalonia.Views
{
    public class SceneObjectProperties : Window
    {
        public SceneObjectProperties()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
