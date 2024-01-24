using System.Net.Sockets;
using System.Threading.Tasks;
using ServerModel.XmlParser.Server;

namespace ClientApp.Models;

public class ClientHandler
{
	private TcpClient Client { get; set; } = new();

	
	public bool IsConnected => Client.Connected; 
	
	public async Task Connect(string ip, int port)
	{
		await Client.ConnectAsync(ip, port);
	}
	
	public async Task Connect()
	{
		await Client.ConnectAsync(ConnectionData.Ip, ConnectionData.Port);
	}
	
	
}