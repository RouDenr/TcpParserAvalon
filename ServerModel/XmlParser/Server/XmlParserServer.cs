using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public class XmlParserServer(IClientsManage clientsManage, IDataProcessor dataProcessor, IParser parser) 
	: AServer(clientsManage, dataProcessor)
{
	IParser Parser { get; } = parser;
	

	public IData ParseFile(FileInfo file)
	{
		return Parser.Parse(file.FullName);
	}
}