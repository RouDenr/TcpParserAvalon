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
	public static ServerManage Instance => _instance ??= new ServerManage();
	private static ServerManage? _instance;
	
	private XmlParserServer? _xmlParserServer;
	
	private Task _serverTask = Task.CompletedTask;
	public ServerManage()
	{
		Console.WriteLine("ServerManage created");
	}
	
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
	
	public async Task SendDataToAll(XmlData data)
	{
		if (!IsRunning) return;
		
		if (_xmlParserServer == null) throw new Exception("Server is not running");
		
		await _xmlParserServer.SendDataToAll(data);
	}

	public void StopServer()
	{
		_xmlParserServer?.Stop();
		_xmlParserServer = null;
		// _serverTask.Wait();
		
		Console.WriteLine("Server stopped");
	}
}