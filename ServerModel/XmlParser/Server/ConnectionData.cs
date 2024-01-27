using System.Net;
using System.Text.Json;

namespace ServerModel.XmlParser.Server;

public class ConnectionData
{
	public int Port { get; private set; } 
	public IPAddress Ip { get; private set; }

	public ConnectionData(string ip, int port)
	{
		Ip = IPAddress.Parse(ip);
		Port = port;
	}
	
	/// <summary>
	/// Config file format: {"ip": "[ip]", "port": "[port]"}
	/// </summary>
	/// <param name="path">Path to config file (expected json)</param>
	/// <exception cref="Exception">Exception thrown if failed to read or deserialize config file</exception>
	public ConnectionData(string path)
	{
		var conf = File.ReadAllText(path);
		if (conf == null)
			throw new FileNotFoundException("Failed to read conf.json");
		
		var confJson = JsonSerializer.Deserialize<Dictionary<string, string>>(conf);
		
		if (confJson == null)
			throw new FormatException("Failed to deserialize conf.json");
		
		Ip = IPAddress.Parse(confJson["ip"]);
		Port = int.Parse(confJson["port"]);
	}

	public ConnectionData()
	{
		
		if (!File.Exists("conf.json"))
		{
			File.WriteAllText("conf.json", CreateDefaultConf());
		}
		var conf = File.ReadAllText("conf.json");


		var confJson = JsonSerializer.Deserialize<Dictionary<string, string>>(conf);
		
		if (confJson == null)
			throw new FormatException("Failed to deserialize conf.json");
		
		Ip = IPAddress.Parse(confJson["ip"]);
		Port = int.Parse(confJson["port"]);
	}

	private string CreateDefaultConf()
	{
		var conf = new Dictionary<string, string>
		{
			{ "ip", "107.0.0.1" },
			{ "port", "8888" }
		};
		
		var json = JsonSerializer.Serialize(conf);
		Console.WriteLine(json);
		return json;
	}
}