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
	
	public UnitTest1()
	{
		_serverMock = new Mock<IServer>();
		
		_serverMock.Setup(x => x.Start()).Verifiable();
		_serverMock.Setup(x => x.Stop()).Verifiable();
		_serverMock.Setup(x => x.Clients).Returns(new List<IDisposable>());
		
		_consoleCommands = new ConsoleCommands(_serverMock.Object);
	}
	
	[Fact]
	public void HelpCommand()
	{
		_consoleCommands.HandleCommand("start");
		
		_serverMock.Verify(x => x.Start(), Times.Once);
	}
	
	[Fact]
	public void ServerStartTest()
	{
		XmlParserServer server = XmlParserServerBuilder.CreateBuilder()
			.SetConnectionData("107.0.0.1", 8888)
			.SetClientHandler()
			.SetDataProcessor(new XmlDataProcessor())
			.SetParser(new XmlParser())
			.Build() as XmlParserServer ?? throw new NullReferenceException("Server is null");
			
		
		if (server is null) throw new ArgumentNullException(nameof(server));
		
		Task start = server.Start();
		
		// Assert
		if (start.IsCompletedSuccessfully)
			Assert.True(server.IsRunning);
		
	}
}