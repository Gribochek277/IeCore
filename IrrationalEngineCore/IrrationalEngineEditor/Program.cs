using Avalonia;
using Avalonia.Logging.Serilog;
using Irrational.Core.Windows;
using IrrationalEngineEditor.ViewModels;
using IrrationalEngineEditor.Views;

namespace IrrationalEngineEditor
{
    class Program
    {
        public static OpenTKWindow context = new OpenTKWindow();
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
