using System.Net.Sockets;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

class ClientHandler : SocketHandler
{
	public string Name { get; set; } = "";
	public ClientHandler(TcpClient socket) : base(socket)
	{
		
		DataReceivedEvent += (_, args) => NamеMessageHandler(args);
		DisconnectedEvent += (_, _) => { Log.Info($"{this} disconnected"); };
	}
	
	private void NamеMessageHandler(IData data)
	{
		if (Name != string.Empty || data is not SimpleTextData nameMessage)
			return;
		// message should be in format: "name: <name>"
		const string format = "name: ";
		if (!nameMessage.Text.StartsWith(format))
			return;
		Name = nameMessage.Text[format.Length..];
	}

	public override string ToString()
	{
		var name = Name != string.Empty ? Name : "Noname";
		return $"{name} {Socket.Client.RemoteEndPoint}";
	}
}