using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public class XmlParserServer(IClientHandler clientHandler, IDataProcessor dataProcessor, IParser parser) 
	: AServer(clientHandler, dataProcessor)
{
	IParser Parser { get; } = parser;
	
	
	public override void Start()
	{
		throw new NotImplementedException();
	}

	public override void Stop()
	{
		throw new NotImplementedException();
	}
}