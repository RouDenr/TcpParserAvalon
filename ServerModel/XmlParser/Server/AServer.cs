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
	
	public IClientsManage ClientsManage { get; }
	public IDataProcessor DataProcessor { get; }
	
	public int Port => ClientsManage.Port;
	public IPAddress Ip => ClientsManage.Ip;
	public bool IsRunning => ClientsManage.IsRunning;
	public IEnumerable<IDisposable> Clients => ClientsManage.Clients;
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();

	public AServer(IClientsManage clientsManage, IDataProcessor dataProcessor)
	{
		ClientsManage = clientsManage;
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
		await ClientsManage.HandleClients();
	}

	public virtual void Stop()
	{
		
		ClientsManage.StopHandle();
	}

	protected virtual void OnServerStartedEvent()
	{
		ServerStartedEvent?.Invoke(this, EventArgs.Empty);
	}
}