using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public sealed class ServerHandler : SocketHandler
{
	
	public static ServerHandler Instance => _instance ??= new ServerHandler();
	public event EventHandler<ServerHandler>? ServerConnectedEvent;
	

	private static ServerHandler? _instance;
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();

	private ServerHandler() : base(new TcpClient())
	{

	}

	private void DisconnectHandler(object? sender, SocketHandler e)
	{
		Socket.Close();
	}

	private Socket? ServerSocket => Socket.Client;

	private IPAddress? Ip { get; set; }
	private int Port { get; set; }

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

	private async Task Connect(IPAddress ip, int port)
	{
		Ip = ip;
		Port = port;
		try {

			Log.Info($"Connecting to {ip}:{port}");
			await Socket.ConnectAsync(ip, port);

			if (!Socket.Connected)
			{
				Log.Error($"Failed to connect to {ip}:{port}");
			}

			Log.Info($"Connected to {ip}:{port}");

			OnServerConnectedEvent(this);
			_ = ReadHandle();
		}
		catch (Exception e)
		{
			Log.Error(e.Message);
			throw;
		}
	}
	
	public async Task Connect(string confPath)
	{
		try {
			ConnectionData connectionData = new(confPath);
			await Connect(connectionData);
		}
		catch (FileNotFoundException e)
		{
			Log.Error(e.Message);
			throw;
		}
		catch (Exception e)
		{
			Log.Error(e.Message);
			throw;
		}
	}
	
	public async Task Connect(ConnectionData connectionData)
	{
		await Connect(connectionData.Ip, connectionData.Port);
	}
	
	public async Task Connect()
	{
		if (Ip == null)
		{
			Log.Error("Ip is null");
			return;
		}
		await Connect(Ip, Port);
	}

	public async Task Reconnect()
	{
		Socket.Close();
		Socket = new TcpClient();

		await Connect();
	}
	
	private void OnServerConnectedEvent(ServerHandler e)
	{
		ServerConnectedEvent?.Invoke(this, e);
	}
}