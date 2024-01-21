namespace ServerModel.XmlParser.ClientModel;

class Client(int id, int port)
	: IClient
{
	public int Id { get; } = id;
	public int Port { get; } = port;
}