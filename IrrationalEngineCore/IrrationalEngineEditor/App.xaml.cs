using Avalonia;
using Avalonia.Markup.Xaml;

namespace IrrationalEngineEditor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
