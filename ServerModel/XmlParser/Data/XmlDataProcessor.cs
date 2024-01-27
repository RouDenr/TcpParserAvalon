namespace ServerModel.XmlParser.Data;

public class XmlDataProcessor(string pathData = "../../../TestResources/") : IDataProcessor
{
	public IEnumerable<IData> ProcessData { get; } = new List<IData>();

	private List<IData> ProcessDataList => ProcessData as List<IData> ?? throw new NullReferenceException();
	public string DataPath { get; set; } = pathData;

	public void Init(string dataDirPath)
	{
		// open DataPath and parse all data
		DataPath = dataDirPath;
		if (!Directory.Exists(dataDirPath))
			throw new DirectoryNotFoundException($"Directory {dataDirPath} not found");
		
		var files = Directory.GetFiles(dataDirPath);
		foreach (var file in files)
		{
			// check if file is valid
			if (!file.EndsWith(".xml"))
				continue;
			
			ProcessDataList.Add(new PathData(file));
		}
	}

	public void Init()
	{
		if (DataPath == null)
			throw new NullReferenceException("DataPath is null");
		Init(!Directory.Exists(DataPath) ? "TestResources/" : DataPath);
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