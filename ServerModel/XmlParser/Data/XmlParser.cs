using System.Drawing;
using System.Xml.Linq;

namespace ServerModel.XmlParser.Data;

public class XmlParser : IParser
{
	private static readonly string[] AllowedImageExtensions = ["bmp"];

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
		return ParseColor(element.Value).ToArgb();
	}

	public static Color ParseColor(string input)
	{
		// #FF00FF format
		if (input.StartsWith($"#"))
		{
			return ColorTranslator.FromHtml(input);
		}
		
		// 255, 0, 255 format
		string[] rgb = input.Split(',');
		
		if (rgb.Length != 3)
		{
			throw new NotParsedException($"Invalid color format {input}");
		}
		return Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
	}
	
	private byte[] GetImage(string name)
	{
		var element = Root.Element(name);
		
		if (element == null)
		{
			throw new NotParsedException($"{name} element is null");
		}
		
		// check attribute mime to image/{allowed extensions}
		var mime = element.Attribute("mime")?.Value;
		if (mime == null)
		{
			throw new NotParsedException($"{name} element has no mime attribute: {element.Value}");
		}
		if (!mime.StartsWith("image/"))
		{
			throw new NotParsedException($"{name} element has invalid mime attribute: {mime}");
		}
		if (!AllowedImageExtensions.Contains(mime["image/".Length..]))
		{
			throw new NotParsedException($"{name} element has invalid mime attribute: {mime}");
		}
		

		return Convert.FromBase64String(element.Value);
	}
}