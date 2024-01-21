namespace ServerModel.XmlParser.Data;

public class XmlDataFactory : IDataFactory
{
	public IDataProcessor CreateDataProcessor()
	{
		var dataProcessor = new XmlDataProcessor();
		
		if (dataProcessor == null)
		{
			throw new NullReferenceException();
		}
		
		return dataProcessor;
	}

	public IParser CreateParser()
	{
		var parser = new XmlParser();
		
		if (parser == null)
		{
			throw new NullReferenceException();
		}
		
		return parser;
	}
}