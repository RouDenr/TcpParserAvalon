namespace ServerModel.XmlParser.Data;

public interface IDataFactory
{
	IDataProcessor CreateDataProcessor();
	IParser CreateParser();
}