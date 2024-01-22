// See https://aka.ms/new-console-template for more information

using System.Net;
using ServerModel.ConsoleImplement;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;


Console.WriteLine("Hello, World!");

IDataFactory dataFactory = new XmlDataFactory();
IClientHandler clientHandler = new TcpClientHandler(port: 8888);
IServer server = new XmlParserServer(clientHandler, dataFactory.CreateDataProcessor(), dataFactory.CreateParser());

server.Start();

ConsoleCommands commands = new(server);

var isRunning = true;
while (isRunning)
{
	try
	{
		Console.Write(">");
		string? command = Console.ReadLine();
		
		commands.HandleCommand(command);
	}
	catch (Exception e)
	{
		Console.WriteLine($"Error: {e.Message}");
	}
}

// Основыные команды
// client <id> <command> - выполнение команды на клиенте
// parse <path> - парсинг файла
// send <user/all> <path> - отправка файла


// exit - выход