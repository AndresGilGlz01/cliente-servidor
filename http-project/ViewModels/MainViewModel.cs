using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using http_project.Models;
using http_project.Services;

using System.Timers;

namespace http_project.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly VmsService _vmsService;

    [ObservableProperty]
    private bool _isServiceRunning;

    private System.Timers.Timer _timer;

    [ObservableProperty]
    private VMS _vms = new();

    public MainViewModel()
    {
        _vmsService = new();
        _vmsService.OnMessageReceived += _vmsService_OnMessageReceived;

        _timer = new(5000);
    }

    private void _vmsService_OnMessageReceived(object? sender, IEnumerable<Models.DTOs.RequestMessageDto> e)
    {
        var count = e.Count();

        var thread = new Thread(() =>
        {
            do
            {
                Vms.Message = e.ElementAt(count - 1).Message;
                OnPropertyChanged(nameof(Vms));

                // Wait for 1 second
                System.Threading.Thread.Sleep(1000);

                count--;

                if (count == 0) count = e.Count();
            } while (count > 0);
        });

        thread.IsBackground = true;
        thread.Start();
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
