// See https://aka.ms/new-console-template for more information

using ServerModel.ConsoleImplement;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

XmlDataFactory dataFactory = new();
IClientHandler clientHandler = new TcpClientsHandler();
XmlParserServer server = new(clientHandler, dataFactory.CreateDataProcessor(), dataFactory.CreateParser());

// Start server
Task serverTask = server.Start();

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

// Основыные команды
// client <id> <command> - выполнение команды на клиенте
// parse <path> - парсинг файла
// send <user/all> <path> - отправка файла


// exit - выход