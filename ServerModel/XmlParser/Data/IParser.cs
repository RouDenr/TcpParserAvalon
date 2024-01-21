namespace ServerModel.XmlParser.Data;

public class NotParsedException(string message) : Exception(message);

public interface IParser
{
	IData Parse(string path);
}