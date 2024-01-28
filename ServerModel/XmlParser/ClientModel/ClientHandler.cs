using System.Net.Sockets;
using NLog;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public class ClientHandler : SocketHandler
{
	public string Name { get; set; } = "";
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	
	public ClientHandler(TcpClient socket) : base(socket)
	{
		
		DataReceivedEvent += (_, args) => {Log.Info($"{this} received data: {args.GetType()}");};
		DisconnectedEvent += (_, _) => { Log.Info($"{this} disconnected"); };
	}
	

	public override string ToString()
	{
		var name = Name != string.Empty ? Name : "Noname";
		return $"{name} {Socket.Client.RemoteEndPoint}";
	}
}