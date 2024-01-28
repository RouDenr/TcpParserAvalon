namespace ServerModel.XmlParser.Data;

[Serializable]
public class XmlData(string path, string? from, string? to, string? text, int color, byte[] image) 
	: PathData(path)
{
	public string? From { get; set; } = from;
	public string? To { get; set; } = to;
	public string? Text { get; set; } = text;
	public int Color { get; set; } = color;
	public byte[] Image { get; set; } = image;
	public int Id { get; } = Count++; 
	private static int Count { get; set; }


	public override IData Clone()
	{
		return new XmlData(Path, From, To, Text, Color, Image);
	}

	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		var other = (XmlData)obj;
		return Color == other.Color &&
		       From == other.From &&
		       Id == other.Id &&
		       Image.SequenceEqual(other.Image) &&
		       Name == other.Name &&
		       Path == other.Path 
			;
	}

	protected bool Equals(XmlData other)
	{
		return From == other.From &&
		       To == other.To &&
		       Text == other.Text &&
		       Color == other.Color &&
		       Image.Equals(other.Image) &&
		       Id == other.Id;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(From, To, Text, Color, Image, Id);
	}
}