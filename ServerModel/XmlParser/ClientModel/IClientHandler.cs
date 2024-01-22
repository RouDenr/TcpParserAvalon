using System.Net;

namespace ServerModel.XmlParser.ClientModel;

public interface IClientHandler
{
	int Port { get; }
	IPAddress Ip { get; }
	IEnumerable<IClient> Clients { get; set; }
	
	IResponseHandler ResponseHandler { get; }
	
	string HandleClient(String request);
	
}