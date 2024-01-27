using ServerModel.XmlParser.Server;

namespace ServerModel.XmlParser;

public class XmlParserServerBuilder : AServerBuilder
{
	public static XmlParserServerBuilder CreateBuilder()
	{
		return new XmlParserServerBuilder();
	}
	
	protected override IServer BuildServer()
	{
		return new XmlParserServer(ClientHandler!, DataProcessor!, Parser!);
	}
}