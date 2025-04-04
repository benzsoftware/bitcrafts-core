using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BitCrafts.Application.Avalonia.Tests;

public partial class AvaloniaApplicationTest : BaseApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }
}