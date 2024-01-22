namespace ServerModel.XmlParser.Data;

public interface IData
{
	IData Clone();
}

public class PathData(string path)
	: IData
{
	public string Path { get; init; } = path;
	
	public IData Clone()
	{
		return new PathData(Path);
	}
}