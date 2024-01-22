using System.Net;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public interface IServer
{
	int Port { get; }
	IPAddress Ip { get; }
	bool IsRunning { get; protected set; }
	
	
	Task Start();
	void Stop();
	IEnumerable<IClient> Clients { get; }
	
	IClientHandler ClientHandler { get; }
	IDataProcessor DataProcessor { get; }
}