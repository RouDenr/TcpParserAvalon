using System.Net;

namespace ServerModel.XmlParser.ClientModel;

public interface IClientsManage
{
	int Port { get; }
	IPAddress Ip { get; }
	IEnumerable<IDisposable> Clients { get; set; }
	
	IRequestHandler RequestHandler { get; }
	bool IsRunning { get;  }

	Task HandleClients();

	void StopHandle();
}