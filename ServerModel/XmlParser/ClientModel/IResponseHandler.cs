namespace ServerModel.XmlParser.ClientModel;

public interface IResponseHandler
{
	string HandleResponse(string dataReceived);
}