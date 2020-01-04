using IrrationalEngineEditor.Models;
using Microsoft.Extensions.Options;
using System.Windows;
using IrrationalEngineEditor.Interfaces.ViewModels;

namespace IrrationalEndgineEditorWpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWindowViewModel _mainWindowViewModel;
        private readonly AppSettings _settings;

        public MainWindow(IMainWindowViewModel mainViewModel,
                      IOptions<AppSettings> settings)
        {
            _mainWindowViewModel = mainViewModel;
            InitializeComponent();
        }

        private void NewWindow(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.NewWindow();
        }
    }
}
