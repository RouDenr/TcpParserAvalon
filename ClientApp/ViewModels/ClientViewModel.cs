namespace ClientApp.ViewModels;

public class ClientViewModel : PageViewModelBase
{
	public string WelcomeMessage { get; } = "Welcome to Avalonia!";
	public override bool CanNavigateNext { get; protected set; } = false;
	public override bool CanNavigatePrevious { get; protected set; } = false;
}