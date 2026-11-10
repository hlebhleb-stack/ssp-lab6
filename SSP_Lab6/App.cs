using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;

namespace SSP_Lab6;

public class App : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new ChartWindow();
            Task3.ChartWindow = window;
            desktop.MainWindow = window;
            _ = Task.Run(async () => await Task3.Run());
        }
        base.OnFrameworkInitializationCompleted();
    }
}
