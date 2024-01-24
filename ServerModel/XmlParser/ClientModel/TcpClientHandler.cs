using System.Net;
using System.Net.Sockets;
using ServerModel.XmlParser.Server;

namespace ServerModel.XmlParser.ClientModel;

public class TcpClientHandler
	: IClientHandler
{
	public int Port { get; }
	public IPAddress Ip { get; }
	public IEnumerable<IDisposable> Clients { get; set; }
	private List<TcpClient> ClientsList { get; }
	
	public IResponseHandler ResponseHandler { get; }
	public bool IsRunning => Listener.Server.IsBound;

	private TcpListener Listener { get; }
	
	public TcpClientHandler()
	{
		Port = ConnectionData.Port;
		Ip = ConnectionData.Ip; 
			
		Listener = new TcpListener(Ip, Port);
		if (Listener == null)
			throw new Exception("Failed to create TcpListener");
		
		Clients = new List<TcpClient>();
		ClientsList = Clients as List<TcpClient> ?? throw new Exception("Failed to cast Clients to List<IClient>");
		ResponseHandler = new TcpResponseHandler();
	}

	public async Task HandleClients()
	{
		// start listening for client connection
		Listener.Start();

		if (!Listener.Server.IsBound)
			throw new Exception("Failed to start listening for client connection");
		Console.WriteLine($"Listening for client connection on {Ip}:{Port}");
		
		while (Listener.Pending())
		{
			// wait for client to connect
			var client = await Listener.AcceptTcpClientAsync() ?? throw new Exception("Failed to accept client");
			ClientsList.Add(client);
		}
	}

	public void StopHandle()
	{
		foreach (var client in Clients)
		{
			client.Dispose();
		}
		
		Listener.Stop();
		ClientsList.Clear();
	}
}