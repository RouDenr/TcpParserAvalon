using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public abstract class AServer(IClientHandler clientHandler, IDataProcessor dataProcessor)
	: IServer
{
	public IClientHandler ClientHandler { get; } = clientHandler;
	public IDataProcessor DataProcessor { get; } = dataProcessor;
	
	public int Port => ClientHandler.Port;
	public int Ip => ClientHandler.Ip;
	public bool IsRunning { get; set; }
	public IEnumerable<IClient> Clients => ClientHandler.Clients;

	public abstract void Start();

	public abstract void Stop();

}