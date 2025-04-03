using Avalonia.Markup.Xaml;
using BitCrafts.Application.Avalonia;

namespace BitCrafts.StartupDemo;

public class App : BaseApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }
}