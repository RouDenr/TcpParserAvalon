namespace ServerModel.XmlParser.ClientModel;

public class TcpResponseHandler : IResponseHandler
{
	public string HandleResponse(string dataReceived)
	{
		return "Response from server: " + dataReceived;
	}
}