using IrrationalEngineCore;
using IrrationalEngineEditor.Implementations;
using IrrationalEngineEditor.Implementations.ViewModels;
using IrrationalEngineEditor.Interfaces;
using IrrationalEngineEditor.Interfaces.ViewModels;
using IrrationalEngineEditor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace IrrationalEndgineEditorWpfUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }
    

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            ServiceCollection serviceCollection = new ServiceCollection();
            IrrationalEngine.RunProgram();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            MainWindow mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>
                    (Configuration.GetSection(nameof(AppSettings)));
            services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<IObjectBrowserViewModel, ObjectBrowserViewModel>();
            services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton(typeof(SceneModel));
            services.AddTransient(typeof(ObjectBrowserViewModel));
            services.AddTransient(typeof(MainWindow));
        }
    }
}
