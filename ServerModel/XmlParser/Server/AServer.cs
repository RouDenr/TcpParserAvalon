using System.Net;
using System.Net.Sockets;
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
	public bool IsRunning => ClientHandler.IsRunning;
	public IEnumerable<IDisposable> Clients => ClientHandler.Clients;

	public virtual async Task Start()
	{
		DataProcessor.Init();
		// start listening for client connection
		await ClientHandler.HandleClients();
	}

	public virtual void Stop()
	{
		
		ClientHandler.StopHandle();
	}

}