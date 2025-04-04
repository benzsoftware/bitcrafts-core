using Avalonia;
using Avalonia.Headless;

namespace BitCrafts.Application.Avalonia.Tests;

public static class AvaloniaTestStartup
{
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<AvaloniaApplicationTest>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions()
            {
                UseHeadlessDrawing = true
            });
    }
}