using ClientApp.Models;
using ReactiveUI;

namespace ClientApp.ViewModels;

public class ClientViewModel : PageViewModelBase
{
	public ClientViewModel()
	{
		ServerInfo = ServerHandler.Instance.ServerInfo();
		ServerHandler.Instance.ServerInfoChangedEvent += (_, _) => ServerInfo = ServerHandler.Instance.ServerInfo();
	}
	
	private string _serverInfo = string.Empty;
	public string ServerInfo { get => _serverInfo; set => this.RaiseAndSetIfChanged(ref _serverInfo, value); }
	public override bool CanNavigateNext { get; protected set; } = false;
	public override bool CanNavigatePrevious { get; protected set; } = false;
}