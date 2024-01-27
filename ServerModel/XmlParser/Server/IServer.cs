using System.Net;
using System.Net.Sockets;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public interface IServer
{
	int Port { get; }
	IPAddress Ip { get; }
	bool IsRunning { get; }
	
	
	Task Start();
	void Stop();
	IEnumerable<IDisposable> Clients { get; }
	
	IClientsManage ClientsManage { get; }
	IDataProcessor DataProcessor { get; }
}