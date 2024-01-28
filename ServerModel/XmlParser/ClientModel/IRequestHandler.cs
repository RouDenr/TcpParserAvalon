using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public interface IRequestHandler
{
	void HandleResponse(object? sender, IData dataReceived);
}