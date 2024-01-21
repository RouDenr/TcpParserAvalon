using System.Net;
using System.Net.Sockets;

namespace ServerModel.XmlParser.ClientModel;

public class TcpClientHandler
	: IClientHandler
{
	public int Port { get; }
	public int Ip { get; }
	public IEnumerable<IClient> Clients { get; set; }
	
	public IResponseHandler ResponseHandler { get; }

	public TcpListener Listener { get; }
	
	public TcpClientHandler(int port, int ip)
	{
		Port = port;
		Ip = ip;
		
		Listener = new TcpListener(new IPAddress(Ip), Port);
		if (Listener == null)
			throw new Exception("Failed to create TcpListener");
		
		Clients = new List<IClient>();
		ResponseHandler = new TcpResponseHandler();
	}

	public string HandleClient(string request)
	{
		throw new NotImplementedException();
	}
}