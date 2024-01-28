namespace ServerModel.XmlParser.ClientModel;

public class TcpRequestHandler : IRequestHandler
{
	public void HandleResponse( string dataReceived)
	{
		var split = dataReceived.Split(" ");
		switch (split[0])
		{
			case "name":
				Console.WriteLine($"Name: {split[1]}");
				break;
		}
	}
}