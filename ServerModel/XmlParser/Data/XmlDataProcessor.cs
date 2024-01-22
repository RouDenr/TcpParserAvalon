namespace ServerModel.XmlParser.Data;

public class XmlDataProcessor : IDataProcessor
{
	private static readonly string DataDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestResources");
	
	public IEnumerable<IData> ProcessData { get; } = new List<IData>();

	private List<IData> ProcessDataList => ProcessData as List<IData> ?? throw new NullReferenceException();
	public string DataPath(string path) => Path.Combine(DataDirPath, path);

	public void Init()
	{
		// open DataPath and parse all data
		var files = Directory.GetFiles(DataDirPath);
		foreach (var file in files)
		{
			// check if file is valid
			if (!file.EndsWith(".xml"))
				continue;
			
			ProcessDataList.Add(new PathData(file));
		}
	}
	
	public void Update()
	{
		// add new data to ProcessData
		var files = Directory.GetFiles(DataDirPath);
		foreach (var file in files)
		{
			// check if file is valid
			if (!file.EndsWith(".xml"))
				continue;

			if (ProcessDataList.Any(data => (data as PathData)?.Path == file))
				continue;
			
			ProcessDataList.Add(new PathData(file));
		}
	}

	public void AddData(IData data)
	{
		ProcessDataList.Add(data);
	}


	public void RemoveData(IData data)
	{
		ProcessDataList.Remove(data);
	}
	

	public void GetData()
	{
		throw new NotImplementedException();
	}
}