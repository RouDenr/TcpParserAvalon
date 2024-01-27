using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ServerApp.Views;

public partial class ServerSettingView : UserControl
{
	public ServerSettingView()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}