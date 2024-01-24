using System.Net;
using System.Net.Sockets;

namespace ServerModel.XmlParser.ClientModel;

public interface IClientHandler
{
	int Port { get; }
	IPAddress Ip { get; }
	IEnumerable<IDisposable> Clients { get; set; }
	
	IResponseHandler ResponseHandler { get; }
	bool IsRunning { get;  }

	Task HandleClients();

	void StopHandle();
}