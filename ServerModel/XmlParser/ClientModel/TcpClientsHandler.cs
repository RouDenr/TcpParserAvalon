using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerModel.XmlParser.Server;

namespace ServerModel.XmlParser.ClientModel;

public class TcpClientsHandler
	: IClientHandler
{
	public int Port { get; }
	public IPAddress Ip { get; }
	public IEnumerable<IDisposable> Clients { get; set; }
	public IResponseHandler ResponseHandler { get; }
	public bool IsRunning => Listener.Server.IsBound;
	
	private List<ClientManage> ClientsList { get; }
	private TcpListener Listener { get; }
	public TcpClientsHandler()
	{
		Port = ConnectionData.Port;
		Ip = ConnectionData.Ip; 
			
		Listener = new TcpListener(Ip, Port);
		if (Listener == null)
			throw new Exception("Failed to create TcpListener");
		
		Clients = new List<ClientManage>();
		ClientsList = Clients as List<ClientManage> ?? throw new Exception("Failed to cast Clients to List<IClient>");
		ResponseHandler = new TcpResponseHandler();
	}

	public async Task HandleClients()
	{
		// start listening for client connection
		Listener.Start();

		if (!Listener.Server.IsBound)
			throw new Exception("Failed to start listening for client connection");
		Console.WriteLine($"Listening for client connection on {Ip}:{Port}");

		try
		{
			while (true)
			{
				// wait for client to connect
				var client = await Listener.AcceptTcpClientAsync() ?? throw new Exception("Failed to accept client");
				ClientManage socketManage = new(client);
				ClientsList.Add(socketManage);
				_ = Task.Run(() => socketManage.ReadHandle());
			}
		}
		finally
		{
			StopHandle();
		}
	}

	public void StopHandle()
	{
		foreach (IDisposable client in Clients)
		{
			client.Dispose();
		}
		
		Listener.Stop();
		ClientsList.Clear();
	}
}