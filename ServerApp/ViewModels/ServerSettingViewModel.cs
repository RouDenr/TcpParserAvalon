using System;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Media;
using ReactiveUI;
using ServerApp.Models;
using ServerModel.XmlParser.Server;

namespace ServerApp.ViewModels;

public class ServerSettingViewModel : ViewModelBase
{

	
	public string ServerName { get => _serverName; set => this.RaiseAndSetIfChanged(ref _serverName, value); }
	public string ServerIp { get => _serverIp; set => this.RaiseAndSetIfChanged(ref _serverIp, value); }
	public string ServerPort { get => _serverPort; set => this.RaiseAndSetIfChanged(ref _serverPort, value); }
	public string ConnectButtonText { get => _connectButtonText; set => this.RaiseAndSetIfChanged(ref _connectButtonText, value); }

	public IImmutableSolidColorBrush ConnectButtonColor { get => _connectButtonColor; set => this.RaiseAndSetIfChanged(ref _connectButtonColor, value); }
	
	public bool IsRunning
	{
		get => _isRunning;
		private set
		{
			this.RaiseAndSetIfChanged(ref _isRunning, value);
			ConnectButtonColor = value ? Brushes.Red : Brushes.Green;
			ConnectButtonText = value ? "Disconnect" : "Connect";
		}
	}
	public ICommand ConnectCommand { get; }

	
	private string _serverName = "Server";
	private string _serverIp = "";
	private string _serverPort = "";
	private string _connectButtonText = "Connect";
	private IImmutableSolidColorBrush _connectButtonColor = Brushes.Green;
	private bool _isRunning;
	
	private readonly ServerManage _serverManage = new();
	
	public ServerSettingViewModel()
	{
		InitServerData();
		
		ConnectCommand = ReactiveCommand.Create(SwitchConnection);
	}

	private void SwitchConnection()
	{
		if (_serverManage.IsRunning)
		{
			_serverManage.StopServer();
			IsRunning = false;
		}
		else
		{
			try
			{
				IsRunning = true;
				_serverManage.StartServer(ServerIp, int.Parse(ServerPort));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				IsRunning = false;
			}
		}
	}

	private void InitServerData()
	{
		ConnectionData connectionData = new ();
		
		ServerIp = connectionData.Ip.ToString();
		ServerPort = connectionData.Port.ToString();
	}
}