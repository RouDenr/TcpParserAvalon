using System.Net;
using System.Text.Json;

namespace ServerModel.XmlParser.Server;

public static class ConnectionData
{
	public static int Port { get; private set; } 
	public static IPAddress Ip { get; private set; }

	private const string ConfJsonFilePath = @"C:\Users\denne\RiderProjects\DotNetStudy\ApiParser\conf.json";

	static ConnectionData()
	{
		var conf = File.ReadAllText(ConfJsonFilePath);
		if (conf == null)
			throw new Exception("Failed to read conf.json");
		
		var confJson = JsonSerializer.Deserialize<Dictionary<string, string>>(conf);
		
		if (confJson == null)
			throw new Exception("Failed to deserialize conf.json");
		
		Ip = IPAddress.Parse(confJson["ip"]);
		Port = int.Parse(confJson["port"]);
	}
}