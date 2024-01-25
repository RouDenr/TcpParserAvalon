namespace ServerModel.XmlParser.Data;

[Serializable]
public class SimpleTextData(string text) : AData
{
	public string Text { get; init; } = text;

	public override IData Clone()
	{
		return new SimpleTextData(Text);
	}
}