using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.Server;

public class XmlParserServer(IClientHandler clientHandler, IDataProcessor dataProcessor, IParser parser) 
	: AServer(clientHandler, dataProcessor)
{
	IParser Parser { get; } = parser;
	
	
	public override Task Start()
	{
		Task task = base.Start();
		if (!task.IsCompletedSuccessfully) return task;
		
		
		
		return task;
	}

	public override void Stop()
	{
		throw new NotImplementedException();
	}

	public IData ParseFile(FileInfo file)
	{
		return Parser.Parse(file.FullName);
	}
}