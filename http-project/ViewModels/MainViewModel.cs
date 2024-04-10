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

    private Thread ShowingThread;
    private IEnumerable<Models.DTOs.RequestMessageDto> _messages;

    public MainViewModel()
    {
        _vmsService = new();
        _vmsService.OnMessageReceived += _vmsService_OnMessageReceived;

        _timer = new(5000);
    }

    private void _vmsService_OnMessageReceived(object? sender, IEnumerable<Models.DTOs.RequestMessageDto> e)
    {
        var count = e.Count();

        _messages = e;

        ShowingThread = new Thread(() =>
        {
            while (count > 0)
            {
                Vms.Message = _messages.ElementAt(count - 1).Message;
                Vms.Pictorama = _messages.ElementAt(count - 1).Pictorama.ToString();
                OnPropertyChanged(nameof(Vms));

                // Wait for 1 second
                System.Threading.Thread.Sleep(5000);

                count--;

                if (count == 0) count = _messages.Count();
            }
        })
        {
            IsBackground = true
        };

        if (!ShowingThread.IsAlive) ShowingThread.Start();
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
