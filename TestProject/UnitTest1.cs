using Moq;
using ServerModel.ConsoleImplement;
using ServerModel.XmlParser;

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
		_serverMock.Setup(x => x.Clients).Returns(new List<IClient>());
		
		_consoleCommands = new ConsoleCommands(_serverMock.Object);
	}
	
	[Fact]
	public void HelpCommand()
	{
		_consoleCommands.HandleCommand("start");
		
		_serverMock.Verify(x => x.Start(), Times.Once);
	}
}