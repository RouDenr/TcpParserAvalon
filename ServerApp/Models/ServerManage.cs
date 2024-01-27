using System;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using ServerModel.XmlParser;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace ServerApp.Models;

public class ServerManage
{
	public bool IsRunning => _xmlParserServer?.IsRunning ?? false;

	public ServerManage()
	{
		Console.WriteLine("ServerManage created");
	}
	
	private XmlParserServer? _xmlParserServer;
	
	private Task _serverTask = Task.CompletedTask;
	
	public void StartServer(string ip, int port)
	{
		if (IsRunning) return;
		
		try {
			XmlParserServerBuilder builder = new();
			builder.SetConnectionData(ip, port)
				.SetClientHandler()
				.SetDataProcessor(new XmlDataProcessor())
				.SetParser(new XmlParser());

			_xmlParserServer = (XmlParserServer) builder.Build();
			_ = _xmlParserServer.Start();
		} 
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	public void StopServer()
	{
		_xmlParserServer?.Stop();
		_xmlParserServer = null;
		// _serverTask.Wait();
		
		Console.WriteLine("Server stopped");
	}
}