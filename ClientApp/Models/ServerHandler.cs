using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public class ServerHandler : SocketHandler
{
	
	public static ServerHandler Instance => _instance ??= new ServerHandler();
	public event EventHandler<ServerHandler>? ServerInfoChangedEvent;
	

	private static ServerHandler? _instance;
	private ServerHandler() : base(new TcpClient()) { }
	private Socket? ServerSocket => Socket.Client;

	public string ServerInfo()
	{
		if (ServerSocket == null)
		{
			Console.WriteLine("Failed to get server info");
			return string.Empty;
		}
		
		StringBuilder sb = new();
		
		sb.AppendLine($"Connected to {ServerSocket.RemoteEndPoint}");
		sb.AppendLine($"Local endpoint: {ServerSocket.LocalEndPoint}");
		sb.AppendLine($"Connected: {ServerSocket.Connected}");
		Console.WriteLine(sb.ToString());
		return sb.ToString();
	}

	public Task Connect(string confPath)
	{
		ConnectionData connectionData = new (confPath);
		Task connect = Socket.ConnectAsync(connectionData.Ip, connectionData.Port);
		
		if (!Socket.Connected)
		{
			Console.WriteLine("Failed to connect");
		}
		Console.WriteLine($"Connected to {Socket.Client.RemoteEndPoint}");
		
		OnServerInfoChanged(this);
		var readHandle = ReadHandle();
		
		return connect;
	}

	protected virtual void OnServerInfoChanged(ServerHandler server)
	{
		ServerInfoChangedEvent?.Invoke(this, server);
	}
}