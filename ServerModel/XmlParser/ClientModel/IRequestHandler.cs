namespace ServerModel.XmlParser.ClientModel;

public interface IRequestHandler
{
	void HandleResponse(string dataReceived);
}