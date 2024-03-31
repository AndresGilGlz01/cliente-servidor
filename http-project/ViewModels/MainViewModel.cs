using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using http_project.Services;

namespace http_project.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly VmsService _vmsService;

    [ObservableProperty]
    private bool _isServiceRunning;

    [ObservableProperty]
    private string _message = string.Empty;

    public MainViewModel()
    {
        _vmsService = new();
    }

    [RelayCommand]
    public void StartService()
    {
        _vmsService.Start();

        IsServiceRunning = true;
    }

    [RelayCommand]
    public void StopService()
    {
        _vmsService.Stop();

        IsServiceRunning = false;
    }
}
