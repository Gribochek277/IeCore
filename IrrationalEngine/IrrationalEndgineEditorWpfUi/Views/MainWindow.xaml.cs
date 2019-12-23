using IrrationalEngineEditor.Interfaces;
using Microsoft.Extensions.Options;
using System.Windows;

namespace IrrationalEndgineEditorWpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISampleService _sampleService;
        private readonly AppSettings _settings;

        public MainWindow(ISampleService sampleService,
                      IOptions<AppSettings> settings)
        {
            InitializeComponent();
            _sampleService = sampleService;
            _settings = settings.Value;
            Title = _sampleService.GetCurrentDate();
        }
    }
}
