using Avalonia;
using Avalonia.ReactiveUI;
using System;
using NLog;

namespace ServerApp;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var logConfig = new NLog.Config.LoggingConfiguration();
		
        var logFile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
        var logConsole = new NLog.Targets.ConsoleTarget("logconsole");
		
        logConfig.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
        logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);
        LogManager.Configuration = logConfig;
        
        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
