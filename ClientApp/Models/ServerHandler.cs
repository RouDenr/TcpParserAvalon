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
	
	private static ServerHandler? _instance;

	private ServerHandler() : base(new TcpClient()) { }
	private Socket? ServerSocket { get; set; }

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

	public Task Connect()
	{
		Task connect = Socket.ConnectAsync(ConnectionData.Ip, ConnectionData.Port);
		
		if (!Socket.Connected)
		{
			Console.WriteLine("Failed to connect");
		}
		Console.WriteLine($"Connected to {Socket.Client.RemoteEndPoint}");
		
		var readHandle = ReadHandle();
		
		return connect;
	}
}