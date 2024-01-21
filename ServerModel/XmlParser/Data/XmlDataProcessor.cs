namespace ServerModel.XmlParser.Data;

public class XmlDataProcessor : IDataProcessor
{
	private const string DataPath = "Data/";
	
	public IEnumerable<IData> ProcessData { get; }
	
	private List<IData> ProcessDataList => ProcessData as List<IData> ?? throw new NullReferenceException();
	private Dictionary<string, IData?> ParsedData { get; } = new();

	public XmlDataProcessor()
	{
		ProcessData = new List<IData>();
	}
	
	public void Init()
	{
		// open DataPath and parse all data
		var files = Directory.GetFiles(DataPath);
		foreach (var file in files)
		{
			// check if file is valid
			if (!file.EndsWith(".xml"))
				continue;
			
			ParsedData.Add(file, null);
		}
	}
	
	public void Update()
	{
		// add new data to ProcessData
		var files = Directory.GetFiles(DataPath);
		foreach (var file in files)
		{
			// check if file is valid
			if (!file.EndsWith(".xml"))
				continue;

			ParsedData.TryAdd(file, null);
		}
	}

	public void AddData(IData data)
	{
		ProcessDataList.Add(data);
	}

	public void AddData(XmlData data)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (data.Path == null)
			throw new ArgumentNullException(nameof(data.Path));
		
		if (!ParsedData.ContainsKey(data.Path))
			throw new KeyNotFoundException($"Data with path {data.Path} not found");
		
		ParsedData[data.Path] = data;
		
		AddData(data as IData);
	}

	public void RemoveData(IData data)
	{
		ProcessDataList.Remove(data);
	}
	
	public void RemoveData(XmlData data)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));
		if (data.Path == null)
			throw new ArgumentNullException(nameof(data.Path));
		
		if (!ParsedData.ContainsKey(data.Path))
			throw new KeyNotFoundException($"Data with path {data.Path} not found");
		
		ParsedData.Remove(data.Path);
		
		RemoveData(data as IData);
	}

	public void GetData()
	{
		throw new NotImplementedException();
	}
}