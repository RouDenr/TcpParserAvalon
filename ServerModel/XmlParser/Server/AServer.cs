using System.Net;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public abstract class AServer(IClientHandler clientHandler, IDataProcessor dataProcessor)
	: IServer
{
	public IClientHandler ClientHandler { get; } = clientHandler;
	public IDataProcessor DataProcessor { get; } = dataProcessor;
	
	public int Port => ClientHandler.Port;
	public IPAddress Ip => ClientHandler.Ip;
	public bool IsRunning { get; set; }
	public IEnumerable<IClient> Clients => ClientHandler.Clients;

	public virtual void Start()
	{
		DataProcessor.Init();
	}

	public abstract void Stop();

}