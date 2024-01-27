// See https://aka.ms/new-console-template for more information

using ServerModel.ConsoleImplement;
using ServerModel.XmlParser;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;
using NLog;

var logConfig = new NLog.Config.LoggingConfiguration();

var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
var logConsole = new NLog.Targets.ConsoleTarget("logconsole");

logConfig.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
LogManager.Configuration = logConfig;



var logger = LogManager.GetCurrentClassLogger();
logger.Info("Program started");

var serverBuilder = new XmlParserServerBuilder();


if (args.Length > 0 && args[0] == "help")
{
	Console.WriteLine("Usage: server <ip> <port>");
	Console.WriteLine("Usage: server <conf.json>");
	Console.WriteLine("Usage: server");
	return;
}

ConnectionData connectionData;
try
{
	connectionData = args.Length switch
	{
		2 => new ConnectionData(args[0], int.Parse(args[1])),
		1 => new ConnectionData(args[0]),
		_ => new ConnectionData()
	};
}
catch (Exception)
{
	logger.Error("Invalid arguments");
	return;
}

try 
{
	serverBuilder.SetConnectionData(connectionData)
	.SetClientHandler(new TcpClientsHandler(connectionData))
	.SetDataProcessor(new XmlDataProcessor())
	.SetParser(new XmlParser());
}
catch (Exception e)
{
	logger.Error(e);
	return;
}

XmlParserServer server;
try
{
	server = serverBuilder.Build() as XmlParserServer ?? throw new NullReferenceException("Server is null");
	logger.Info("Server created");
}
catch (Exception e)
{
	logger.Error(e);
	return;
}

// Start server
_ = server.Start();
ConsoleCommands commands = new(server);

while (true)
{
	try
	{
		Console.Write(">");
		var command = Console.ReadLine();
		
		commands.HandleCommand(command);
	}
	catch (Exception e)
	{
		logger.Error(e);
	}
}