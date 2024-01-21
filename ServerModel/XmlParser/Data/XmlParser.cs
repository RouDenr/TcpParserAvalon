using System.Xml.Linq;

namespace ServerModel.XmlParser.Data;

public class XmlParser : IParser
{
	private XElement? _root;
	private XElement Root
	{
		get
		{
			if (_root == null)
			{
				throw new NotParsedException("Root element is null");
			}
			return _root;
		}
	}
	
	
	public IData Parse(string path)
	{
		// parse xml file to get data
		XDocument doc = XDocument.Load(path);

		_root = doc.Root ?? throw new NotParsedException("Root element is null");
		var element = Root.Element("imp_path");
		if (element != null)
		{
			AddXmlImgBmp(path, element.Value);
		}
		XmlData data = new (
			path: path,
			from: GetString("from"),
			to : GetString("to"),
			text: GetString("text"),
			color: GetInt("color"),
			image: GetImage("image")
			);


		return data;
	}

	/// <summary>
	/// Add image to xml file
	/// </summary>
	/// <param name="path"></param>
	/// <param name="elementValue"></param>
	/// <exception cref="NotParsedException"></exception>
	private void AddXmlImgBmp(string path, string elementValue)
	{
		if (!elementValue.EndsWith(".bmp"))
		{
			throw new NotParsedException($"Image {elementValue} is not bmp");
		}
		
		var imgPath = Path.Combine(Path.GetDirectoryName(path) ?? throw new NotParsedException("Path is null"), elementValue);
		
		if (!File.Exists(imgPath))
		{
			throw new NotParsedException($"Image {imgPath} not found");
		}
		byte[] img = File.ReadAllBytes(imgPath);
		
		if (Root.Element("image") != null)
		{
			Root.Element("image")?.Remove();
		}
		Root.Add(new XElement("image", Convert.ToBase64String(img)));
	}

	private string GetString(string name)
	{
		var element = Root.Element(name);
		
		if (element == null)
		{
			throw new NotParsedException($"{name} element is null");
		}
		return element.Value;
	}
	
	private int GetInt(string name)
	{
		var element = Root.Element(name);
		
		if (element == null)
		{
			throw new NotParsedException($"{name} element is null");
		}
		return int.Parse(element.Value);
	}
	
	private byte[] GetImage(string name)
	{
		var element = Root.Element(name);
		
		if (element == null)
		{
			throw new NotParsedException($"{name} element is null");
		}

		return Convert.FromBase64String(element.Value);
	}
}