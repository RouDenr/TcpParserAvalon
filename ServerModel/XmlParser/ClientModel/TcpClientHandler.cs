using System.Net;
using System.Net.Sockets;
using System.Text;
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
			
			Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
			await HandleClient(client);
		}
	}

	private async Task HandleClient(TcpClient client)
	{
		await using NetworkStream stream = client.GetStream();
		var buffer = new byte[client.ReceiveBufferSize];
		
		while (client.Connected)
		{
			// read data from client
			var bytesRead = await stream.ReadAsync(buffer.AsMemory(0, client.ReceiveBufferSize));
			if (bytesRead == 0)
				break;
			
			// convert data to string
			var dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
			Console.WriteLine($"Received: {dataReceived}");
			
			// send response to client
			var response = ResponseHandler.HandleResponse(dataReceived);
			var dataSent = Encoding.ASCII.GetBytes(response);
			stream.Write(dataSent, 0, dataSent.Length);
			Console.WriteLine($"Sent: {response}");
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