using System.Reflection;
using ServerModel.XmlParser;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Server;

namespace ServerModel.ConsoleImplement;


public class ConsoleCommands
{
	private readonly IServer _server;
	private readonly Dictionary<string, ConsoleCommandDelegate> _commands = new();
	public ConsoleCommands(IServer server)
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
	public void StartServer(object[] args)
	{
		_server.Start();
	}
	
	[ConsoleCommand("stop", "Stops the server")]
	public void StopServer(object[] args)
	{
		_server.Stop();
	}

	[ConsoleCommand("clients", "Shows all connected clients")]
	public void ShowClients(object[] args)
	{
		foreach (IClient client in _server.Clients)
		{
			Console.WriteLine(client);
		}
	}
	
	[ConsoleCommand("client", "Shows a specific client", "id")]
	public void ShowClient(object[] args)
	{
		var id = args[0].ToString();
		if (string.IsNullOrWhiteSpace(id))
		{
			Console.WriteLine("Invalid id");
			return;
		}
		
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
	public void Help(object[] args)
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

	public void HandleCommand(string command)
	{
		if (string.IsNullOrWhiteSpace(command)) return;
		
		// Split the command into the command name and the arguments
		var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0) return;
		
		// Get the command name
		var commandName = parts[0];
		if (!_commands.ContainsKey(commandName))
		{
			Console.WriteLine("Unknown command");
			return;
		}
		
		// Get the command delegate
		var commandDelegate = _commands[commandName];
		ConsoleCommandAttribute? attribute = commandDelegate.Method.GetCustomAttribute<ConsoleCommandAttribute>();
		if (attribute == null) return;
		
		if (parts.Length - 1 != attribute.Args.Length)
		{
			Console.WriteLine("Invalid arguments");
			return;
		}
		
		// Invoke the command delegate
		commandDelegate(parts.Skip(1).Select(x => (object)x).ToArray());
	}
}