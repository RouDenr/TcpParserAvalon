namespace ServerModel.XmlParser.Data;

public class PathData(string path)
	: AData
{
	public string Path { get; init; } = path;
	public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
	
	public override IData Clone()
	{
		return new PathData(Path);
	}
}