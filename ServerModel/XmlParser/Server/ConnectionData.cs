using System.Net;

namespace ServerModel.XmlParser.Server;

public static class ConnectionData
{
	public static int Port => 8080;
	public static IPAddress Ip { get; } = IPAddress.Parse("127.0.0.1");
}