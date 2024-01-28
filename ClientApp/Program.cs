using Avalonia;
using Avalonia.ReactiveUI;
using System;
using NLog;

namespace ClientApp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var logconfig = new NLog.Config.LoggingConfiguration();
        var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
        var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
        logconfig.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
        logconfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
        NLog.LogManager.Configuration = logconfig;
        
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
