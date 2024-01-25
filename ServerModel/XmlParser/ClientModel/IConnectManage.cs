using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public interface IConnectManage : IDisposable
{
	event EventHandler<IData> DataReceivedEvent;
	event EventHandler<SocketHandler>? DisconnectedEvent;

	Task ReadHandle();
	Task<IData?> ReadDataAsync();
	Task SendDataAsync(IData data);
}