using System.Reflection;
using ServerModel.XmlParser;

namespace ServerModel.ConsoleImplement;


class ConsoleCommands
{
	private readonly IServer _server;
	private Dictionary<string, ConsoleCommandDelegate> _commands = new();
	
	ConsoleCommands(IServer server)
	{
		_server = server;
		TypeInfo typeInfo = typeof(ConsoleCommands).GetTypeInfo();
		foreach (MethodInfo method in typeInfo.DeclaredMethods)
		{
			ConsoleCommandAttribute? attribute = method.GetCustomAttribute<ConsoleCommandAttribute>();
			if (attribute == null) continue;
			
			// Add all methods with ConsoleCommandAttribute to the dictionary
			_commands.Add(attribute.Name, (ConsoleCommandDelegate)method.CreateDelegate(typeof(ConsoleCommandDelegate), this));
		}
	}
	
	[ConsoleCommand("start", "Starts the server")]
	public void StartServer()
	{
		_server.Start();
	}
	
	[ConsoleCommand("stop", "Stops the server")]
	public void StopServer()
	{
		_server.Stop();
	}

	[ConsoleCommand("clients", "Shows all connected clients")]
	public void ShowClients()
	{
		foreach (IClient client in _server.Clients)
		{
			Console.WriteLine(client);
		}
	}
	
	[ConsoleCommand("client", "Shows a specific client", "id")]
	public void ShowClient(string id)
	{
		if (!int.TryParse(id, out var clientId))
		{
			Console.WriteLine("Invalid id");
			return;
		}
		
		IClient? client = _server.Clients.FirstOrDefault(c => c.Id == clientId);
		if (client == null)
		{
			Console.WriteLine("Client not found");
			return;
		}
		
		Console.WriteLine(client.ToString());
	}
	
	[ConsoleCommand("help", "Shows this help")]
	public void Help()
	{
		foreach (KeyValuePair<string, ConsoleCommandDelegate> command in _commands)
		{
			ConsoleCommandAttribute? attribute = command.Value.Method.GetCustomAttribute<ConsoleCommandAttribute>();
			if (attribute == null) continue;
			
			Console.WriteLine($"{attribute.Name} - {attribute.Description}");
			
			if (attribute.Args.Length == 0) continue;
			Console.WriteLine($"Usage: {attribute.Name} {string.Join(" ", attribute.Args)}");
		}
	}
	
}