using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using ClientApp.Models;
using ReactiveUI;
using ServerApp.Models;
using ServerModel.XmlParser.Data;

namespace ClientApp.ViewModels;

public class ClientViewModel : PageViewModelBase
{
	private XmlData? _dataInfo;
	private Bitmap? _image;
	private StringBuilder _log = new();

	private bool _isResendEnabled;
	
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
	
	public ICommand ResendCommand { get; }
	
	
	public ClientViewModel()
	{
		ResendCommand = ReactiveCommand.CreateFromTask(Resend);
		
		ServerHandler.Instance.DataReceivedEvent += HandleResponse;
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
	
	