using Avalonia;
using Avalonia.Markup.Xaml;
using IrrationalEngineEditor.ViewModels;

namespace IrrationalEngineEditor.Avalonia.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
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
