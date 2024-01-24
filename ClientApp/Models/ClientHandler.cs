using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public class ClientHandler
{
	public static ClientHandler Instance => _instance ??= new();
	
	private static ClientHandler? _instance;
	private TcpClient Client { get; set; } = new();
	private NetworkStream Stream => Client.GetStream();

	
	public bool IsConnected => Client.Connected; 
	
	public async Task Connect(string ip, int port)
	{
		await Client.ConnectAsync(ip, port);
	}
	
	public async Task Connect()
	{
		Console.WriteLine($"Connecting to {ConnectionData.Ip}:{ConnectionData.Port}");
		await Client.ConnectAsync(ConnectionData.Ip, ConnectionData.Port);

		Console.WriteLine(Client.Connected ? "Connected" : "Failed to connect");
		
		await SendData("Hello from client");
	}

	private async Task SendData(object data)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(data.ToString() ?? string.Empty);
		await Stream.WriteAsync(buffer.AsMemory(0, buffer.Length));
	}

	public string ServerInfo()
	{
		StringBuilder sb = new();
		
		sb.AppendLine($"Connected to {Client.Client.RemoteEndPoint}");
		sb.AppendLine($"Local address: {Client.Client.LocalEndPoint}");
		sb.AppendLine($"Available: {Client.Available}");
		sb.AppendLine($"Connected: {Client.Connected}");
		sb.AppendLine($"Exclusive address use: {Client.ExclusiveAddressUse}");
		return sb.ToString();
	}
	
	private ClientHandler()
	{
	}
	
}