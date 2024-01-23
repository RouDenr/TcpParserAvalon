using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ServerApp.Models;
using ServerModel.XmlParser.Data;

namespace ServerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private XmlData? _dataInfo;
    private Bitmap? _image;
    private StringBuilder _log = new();

    public MainWindowViewModel()
    {
        SelectFileCommand = ReactiveCommand.CreateFromTask(SelectFile);
    }
    
    // TODO: change obsolete method
    [Obsolete("Obsolete")]
    private async Task SelectFile()
    {
        //Window.StorageProvider API 
        var dialog = new OpenFileDialog();
        var result = await dialog.ShowAsync(new Window());
        if (result != null)
        {
            FileInfo file = new(result[0]);
            DataInfo = ServerInstance.Instance.ParseFile(file);
        }
    }
    
    
#pragma warning disable CA1822 // Mark members as static
    // MVVM pattern
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
    public ICommand SelectFileCommand { get; }
    public StringBuilder Log { get => _log; set => this.RaiseAndSetIfChanged(ref _log, value); }
#pragma warning restore CA1822 // Mark members as static
}
