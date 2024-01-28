using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ClientApp.Models;
using NLog;
using ReactiveUI;
using ServerApp.Models;
using ServerModel.XmlParser.ClientModel;
using ServerModel.XmlParser.Data;

namespace ClientApp.ViewModels;

public class ClientViewModel : PageViewModelBase
{
	private XmlData? _dataInfo;
	private Bitmap? _image;
	private StringBuilder _log = new();
	private string _connectionStatus = "Disconnected";
	private SolidColorBrush _connectionStatusColor  = new (Colors.Red);

	private bool _isResendEnabled;
	private bool _isReconnectEnabled;
	
	private static readonly Logger Loger = LogManager.GetCurrentClassLogger();
	
	public XmlData? DataInfo { 
		get => _dataInfo;
		private set
		{
			this.RaiseAndSetIfChanged(ref _dataInfo, value);
			Image = BitmapConverter.ConvertBase64ToBitmap(DataInfo.Image);
		}
	}
	
	public Bitmap? Image
	{
		get => _image;
		set => this.RaiseAndSetIfChanged(ref _image, value);
	}
	public StringBuilder Log
	{
		get => _log;
		set => this.RaiseAndSetIfChanged(ref _log, value);
	}
	public bool IsResendEnabled { 
		get => _isResendEnabled;
		set => this.RaiseAndSetIfChanged(ref _isResendEnabled, value);
	}
	
	public string ConnectionStatus
	{
		get => _connectionStatus;
		set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
	}
	public SolidColorBrush ConnectionStatusColor
	{
		get => _connectionStatusColor;
		set => this.RaiseAndSetIfChanged(ref _connectionStatusColor, value);
	}
	public bool IsReconnectEnabled 
	{ 
		get => _isReconnectEnabled;
		set => this.RaiseAndSetIfChanged(ref _isReconnectEnabled, value);
	}
	public ICommand ResendCommand { get; }
	public ICommand ReconnectCommand { get; }
	
	
	public ClientViewModel()
	{
		ResendCommand = ReactiveCommand.CreateFromTask(Resend);
		ReconnectCommand = ReactiveCommand.CreateFromTask(Reconnect);
		
		ServerHandler.Instance.DataReceivedEvent += HandleResponse;
		ServerHandler.Instance.DisconnectedEvent += HandleDisconnect;
		ServerHandler.Instance.ServerConnectedEvent += HandleConnect;
	}

	private async Task Reconnect()
	{
		Loger.Info("Reconnecting...");
		
		await ServerHandler.Instance.Reconnect();
	}

	private void HandleConnect(object? sender, ServerHandler e)
	{
		IsReconnectEnabled = false;
		IsResendEnabled = false;
		ConnectionStatus = "Connected";
		ConnectionStatusColor = new SolidColorBrush(Colors.Green);
	}

	private void HandleDisconnect(object? sender, SocketHandler e)
	{
		IsReconnectEnabled = true;
		IsResendEnabled = false;
		ConnectionStatus = "Disconnected";
		ConnectionStatusColor = new SolidColorBrush(Colors.Red);
	}

	private void HandleResponse(object? sender, IData e)
	{
		if (e is XmlData data)
		{
			DataInfo = data;
			IsResendEnabled = true;
		}
	}

	private async Task Resend()
	{
		if (DataInfo is null)
		{
			throw new Exception("DataInfo is null");
		}
		
		SimpleTextData simpleTextData = new("resend");
		
		await ServerHandler.Instance.SendDataAsync(simpleTextData);
	}

	public override bool CanNavigateNext { get; protected set; } = false;
	public override bool CanNavigatePrevious { get; protected set; } = false;
}
	
	