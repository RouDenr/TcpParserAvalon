using System.Reactive;
using Avalonia.Media;
using ReactiveUI;

namespace ClientApp.ViewModels;

public class LoginViewModel : PageViewModelBase
{
    private string _username = string.Empty;
    private string _connectionStatus = "Disconnected";
    private SolidColorBrush _connectionStatusColor  = new (Colors.Red);
	
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
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
	
    public ReactiveCommand<Unit, Unit> ConnectCommand { get; }
	
	
    public LoginViewModel()
    {
        ConnectCommand = ReactiveCommand.Create(Connect);
    }

    private void Connect()
    {
        bool isUsernameValid = !string.IsNullOrWhiteSpace(Username);
		
        if (isUsernameValid)
        {
            ConnectionStatus = "Connected";
            ConnectionStatusColor = new SolidColorBrush(Colors.Green);
            CanNavigateNext = true;
        }
        else
        {
            ConnectionStatus = "Disconnected";
            ConnectionStatusColor = new SolidColorBrush(Colors.Red);
        }
    }


    public override bool CanNavigateNext { get; protected set; }
    public override bool CanNavigatePrevious { get; protected set; } = false;
}
