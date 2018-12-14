using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IrrationalEngineEditor.Avalonia.Pages
{
    public class EngineWindow : UserControl
    {
        public EngineWindow()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
