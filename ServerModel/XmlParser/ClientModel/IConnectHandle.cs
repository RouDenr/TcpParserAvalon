using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public interface IConnectHandle : IDisposable
{
	event EventHandler<IData> DataReceivedEvent;
	event EventHandler<SocketHandler>? DisconnectedEvent;

	Task ReadHandle();
	Task<IData?> ReadDataAsync();
	Task SendDataAsync(IData data);
}