using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public class ServerHandler : SocketHandler
{
	
	public static ServerHandler Instance => _instance ??= new ServerHandler();
	public event EventHandler<ServerHandler>? ServerInfoChangedEvent;
	

	private static ServerHandler? _instance;
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	
	private ServerHandler() : base(new TcpClient()) { }
	private Socket? ServerSocket => Socket.Client;

	public string ServerInfo()
	{
		if (ServerSocket == null)
		{
			Log.Error("Server socket is null");
			return string.Empty;
		}
		
		StringBuilder sb = new();
		
		sb.AppendLine($"Connected to {ServerSocket.RemoteEndPoint}");
		sb.AppendLine($"Local endpoint: {ServerSocket.LocalEndPoint}");
		sb.AppendLine($"Connected: {ServerSocket.Connected}");
		Console.WriteLine(sb.ToString());
		return sb.ToString();
	}

	private Task Connect(ConnectionData connectionData)
	{
		Task connect = Socket.ConnectAsync(connectionData.Ip, connectionData.Port);
		
		if (!Socket.Connected)
		{
			Log.Error($"Failed to connect to {connectionData.Ip}:{connectionData.Port}");
			return connect;
		}
		Log.Info($"Connected to {connectionData.Ip}:{connectionData.Port}");
		
		OnServerInfoChanged(this);
		_ = ReadHandle();
		
		return connect;
	}
	
	public Task Connect(string confPath)
	{
		ConnectionData connectionData = new (confPath);
		return Connect(connectionData);
	}
	
	public Task Connect(string ip, int port)
	{
		ConnectionData connectionData = new (ip, port);
		return Connect(connectionData);
	}

	protected virtual void OnServerInfoChanged(ServerHandler server)
	{
		ServerInfoChangedEvent?.Invoke(this, server);
	}
}