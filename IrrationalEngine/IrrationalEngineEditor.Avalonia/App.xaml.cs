using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using IrrationalEngineEditor.Avalonia.Views;
using IrrationalEngineEditor.ViewModels;
using Serilog;

namespace IrrationalEngineEditor.Avalonia
{
    internal class App : Application
    {
        private static void Main(string[] args)
        {
            InitializeLogging();
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp() =>
           AppBuilder.Configure<App>()
               .UseReactiveUI()
               .UsePlatformDetect();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }

    }
}
