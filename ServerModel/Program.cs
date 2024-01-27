// See https://aka.ms/new-console-template for more information

using ServerModel.ConsoleImplement;
using ServerModel.XmlParser;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

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
		_ => new ConnectionData("conf.json")
	};
}
catch (Exception)
{
	Console.WriteLine("Failed to load config file. Using default values");
	return;
}

serverBuilder.SetConnectionData(connectionData)
	.SetClientHandler(new TcpClientsHandler(connectionData))
	.SetDataProcessor(new XmlDataProcessor())
	.SetParser(new XmlParser());


XmlParserServer server = serverBuilder.Build() as XmlParserServer ?? throw new NullReferenceException("Server is null");

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
		Console.WriteLine($"Error: {e}");
	}
}