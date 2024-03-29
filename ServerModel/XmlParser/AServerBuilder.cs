﻿using NLog;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ServerModel.XmlParser;

public abstract class AServerBuilder
{
	private ConnectionData? ConnectionData { get; set; }
	protected IClientsManage? ClientHandler { get; set; }
	protected IDataProcessor? DataProcessor { get; set; }
	protected IParser? Parser { get; set; }
	
	public static readonly Logger Log = LogManager.GetCurrentClassLogger();
	
	// Setters
	public AServerBuilder SetConnectionData(ConnectionData connectionData)
	{
		ConnectionData = connectionData;
		return this;
	}
	public AServerBuilder SetConnectionData(string ip, int port)
	{
		ConnectionData = new ConnectionData(ip, port);
		return this;
	}
	public AServerBuilder SetConnectionData(string path)
	{
		ConnectionData = new ConnectionData(path);
		return this;
	}
	
	public AServerBuilder SetClientHandler(IClientsManage clientsManage)
	{
		ClientHandler = clientsManage;
		return this;
	}
	public AServerBuilder SetClientHandler()
	{
		if (ConnectionData is null)
		{
			throw new NullReferenceException("Connection data is not set");
		}
		ClientHandler = new TcpClientsManage(ConnectionData);
		return this;
	}
	
	public AServerBuilder SetDataProcessor(IDataProcessor dataProcessor)
	{
		DataProcessor = dataProcessor;
		return this;
	}
	
	public AServerBuilder SetParser(IParser parser)
	{
		Parser = parser;
		return this;
	}
	
	/// <summary>
	/// Set connection data
	/// </summary>
	/// <returns>Current builder</returns>
	/// <exception cref="NullReferenceException"> Throws if any connection data is null</exception>
	public IServer Build()
	{
		if (ClientHandler is null)
		{
			Log.Error("Client handler is not set");
			throw new NullReferenceException("Client handler is not set");
		}
		
		if (DataProcessor is null)
		{
			Log.Error("Data processor is not set");
			throw new NullReferenceException("Data processor is not set");
		}
		
		if (Parser is null)
		{
			Log.Error("Parser is not set");
			throw new NullReferenceException("Parser is not set");
		}
		
		return BuildServer();
	} 
	
	protected abstract IServer BuildServer();
}