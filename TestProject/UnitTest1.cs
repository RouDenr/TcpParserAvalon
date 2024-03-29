using System.Drawing;
using Moq;
using ServerModel.ConsoleImplement;
using ServerModel.XmlParser;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;
using ServerModel.XmlParser.Server;

namespace TestProject;

public class UnitTest1
{
	private readonly ConsoleCommands _consoleCommands;
	private readonly Mock<IServer> _serverMock;
	private readonly Mock<ConnectionData> _connectionDataMock;
	private readonly Mock<IClientsManage> _clientHandlerMock;
	private readonly Mock<IDataProcessor> _dataProcessorMock;
	private readonly Mock<IParser> _parserMock;
	
	public UnitTest1()
	{
		_serverMock = new Mock<IServer>();
		
		_serverMock.Setup(x => x.Start()).Verifiable();
		_serverMock.Setup(x => x.Stop()).Verifiable();
		_serverMock.Setup(x => x.Clients).Returns(new List<IDisposable>());
		
		_consoleCommands = new ConsoleCommands(_serverMock.Object);
		
		_connectionDataMock = new Mock<ConnectionData>();
		_clientHandlerMock = new Mock<IClientsManage>();
		_dataProcessorMock = new Mock<IDataProcessor>();
		_parserMock = new Mock<IParser>();
	}
	
	[Fact]
	public void HelpCommand()
	{
		_consoleCommands.HandleCommand("start");
		
		_serverMock.Verify(x => x.Start(), Times.Once);
	}
	
	[Fact]
	public void InvalidServerBuilderTest()
	{
		XmlParserServerBuilder builder = XmlParserServerBuilder.CreateBuilder();
		
		Assert.Throws<NullReferenceException>(() => builder.Build());

		Assert.Throws<NullReferenceException>(() => builder.SetClientHandler());
		
		builder.SetConnectionData("107.0.0.1", 8888);
		Assert.Throws<NullReferenceException>(() => builder.Build());
		
		builder.SetClientHandler();
		Assert.Throws<NullReferenceException>(() => builder.Build());
		
		builder.SetDataProcessor(new XmlDataProcessor());
		Assert.Throws<NullReferenceException>(() => builder.Build());
		
		builder.SetParser(new XmlParser());
		// All set
		Assert.NotNull(builder.Build());
	}

	[Fact]
	public void ServerBuilderTest()
	{
		XmlParserServerBuilder builder = XmlParserServerBuilder.CreateBuilder();

		Assert.Throws<NullReferenceException>(() => builder.Build());

		Assert.Throws<NullReferenceException>(() => builder.SetClientHandler());

		builder.SetConnectionData(new ConnectionData("107.0.0.1", 8888));
		Assert.Throws<NullReferenceException>(() => builder.Build());

		builder.SetClientHandler();
		Assert.Throws<NullReferenceException>(() => builder.Build());

		builder.SetDataProcessor(new XmlDataProcessor());
		Assert.Throws<NullReferenceException>(() => builder.Build());

		builder.SetParser(new XmlParser());
		// All set
		Assert.NotNull(builder.Build());
	}
	[Fact]
	public void ServerBuilderTestNewClient()
	{
		XmlParserServerBuilder builder = XmlParserServerBuilder.CreateBuilder();

		builder.SetClientHandler(new TcpClientsManage(new ConnectionData()));
		Assert.Throws<NullReferenceException>(() => builder.Build());

		builder.SetDataProcessor(new XmlDataProcessor());
		Assert.Throws<NullReferenceException>(() => builder.Build());

		builder.SetParser(new XmlParser());
		// All set
		Assert.NotNull(builder.Build());
	}

	[Fact]
	public void NotValidConnectionData()
	{
		Assert.Throws<FileNotFoundException>(() => new ConnectionData("invalid.json"));

		XmlParserServerBuilder builder = XmlParserServerBuilder.CreateBuilder();
		Assert.Throws<FileNotFoundException>(() => builder.SetConnectionData("invalid.json"));
		
		builder.SetConnectionData("conf.json")
			.SetClientHandler()
			.SetDataProcessor(new XmlDataProcessor())
			.SetParser(new XmlParser());
		Assert.NotNull(builder.Build());
	}


	[Fact]
	public void XmlSerializeTest()
	{
		XmlData xmlData = new(
			path: "path",
			from: "from",
			to: "to",
			text: "text",
			color: Color.Green.ToArgb(),
			image: new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
		);
		var xml = xmlData.Serialize();
		var deserialized = AData.Deserialize(xml);

		Assert.NotNull(deserialized);
		Assert.True(deserialized is XmlData);
		Assert.Equal(xmlData, deserialized);
	}
	
	[Fact]
	public void XmlFileSerializeTest()
	{
		var parser = new XmlParser();
		
		XmlData xmlData = parser.Parse("../../../TestResources/test1.xml") as XmlData ?? throw new Exception("Failed to parse");
		var xml = xmlData.Serialize();
		var deserialized = AData.Deserialize(xml);

		Assert.NotNull(deserialized);
		Assert.True(deserialized is XmlData);
		Assert.Equal(xmlData, deserialized);
	}
}