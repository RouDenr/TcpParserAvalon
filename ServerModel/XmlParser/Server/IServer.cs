using ServerModel.XmlParser.ClientModel;

namespace ServerModel.XmlParser.Server;

public interface IServer
{
	int Port { get; }
	int Ip { get; }
	bool IsRunning { get; protected set; }
	
	
	void Start();
	void Stop();
	IEnumerable<IClient> Clients { get; }
	
	IClientHandler ClientHandler { get; }
	IDataProcessor DataProcessor { get; }
}