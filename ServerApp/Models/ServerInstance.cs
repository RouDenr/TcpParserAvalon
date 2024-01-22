using System;
using System.IO;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ServerApp.Models;

public class ServerInstance
{
	public XmlParserServer ParseServer => Server as XmlParserServer ?? throw new NullReferenceException("Server is null");
	public TcpClientHandler TcpClients => Clients as TcpClientHandler ?? throw new NullReferenceException("Clients is null");
	
	private IServer Server { get; }
	private IClientHandler Clients { get; }
	
	
	
	public static ServerInstance Instance { get; } = new ();

	private ServerInstance()
	{
		XmlDataFactory factory = new ();
		Clients = new TcpClientHandler(port: 8888);
		Server = new XmlParserServer(Clients, factory.CreateDataProcessor(), factory.CreateParser());
	}

	public XmlData ParseFile(FileInfo file)
	{
		return (XmlData) ParseServer.ParseFile(file);
	}
}