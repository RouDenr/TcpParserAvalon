using System.Net;
using System.Net.Sockets;
using NLog;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public abstract class AServer
	: IServer
{
	public event EventHandler ServerStartedEvent;
	
	public IClientHandler ClientHandler { get; }
	public IDataProcessor DataProcessor { get; }
	
	public int Port => ClientHandler.Port;
	public IPAddress Ip => ClientHandler.Ip;
	public bool IsRunning => ClientHandler.IsRunning;
	public IEnumerable<IDisposable> Clients => ClientHandler.Clients;
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();

	public AServer(IClientHandler clientHandler, IDataProcessor dataProcessor)
	{
		ClientHandler = clientHandler;
		DataProcessor = dataProcessor;
		
		ServerStartedEvent += (_,_)=>
		{
			Log.Info($"Started server on {Ip}:{Port}");
		};
	}
	
	public virtual async Task Start()
	{
		await Task.Yield();
		
		DataProcessor.Init();
		OnServerStartedEvent();
		
		// start listening for client connection
		await ClientHandler.HandleClients();
	}

	public virtual void Stop()
	{
		
		ClientHandler.StopHandle();
	}

	protected virtual void OnServerStartedEvent()
	{
		ServerStartedEvent?.Invoke(this, EventArgs.Empty);
	}
}