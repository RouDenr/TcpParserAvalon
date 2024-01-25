using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public class ServerHandler : SocketManage
{
	
	public static ServerHandler Instance => _instance ??= new ServerHandler();
	
	private static ServerHandler? _instance;

	private ServerHandler() : base(new TcpClient()) { }

	public string ServerInfo()
	{
		StringBuilder sb = new();
		
		sb.AppendLine($"Connected to {Socket.Client.RemoteEndPoint}");
		sb.AppendLine($"Local address: {Socket.Client.LocalEndPoint}");
		sb.AppendLine($"Available: {Socket.Available}");
		sb.AppendLine($"Connected: {Socket.Connected}");
		sb.AppendLine($"Exclusive address use: {Socket.ExclusiveAddressUse}");
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
		
		Task.Run(async () => await ReadHandle());
		
		return connect;
	}
}