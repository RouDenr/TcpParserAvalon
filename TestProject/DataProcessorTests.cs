using ServerModel.XmlParser.Data;

namespace TestProject;

public class DataProcessorTests
{
	private readonly XmlDataProcessor _processor = new();
	
	public DataProcessorTests()
	{
		// Mock data
	}
	
	[Fact]
	public void InitTest()
	{
		_processor.Init();
		
		var files = Directory.GetFiles(XmlDataProcessor.DataDirPath);
		var xmlFiles = files.Where(file => file.EndsWith(".xml")).ToList();
		
		Assert.Equal(xmlFiles.Count, _processor.ProcessData.Count());
	}
}