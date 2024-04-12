using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using http_project.Models;
using http_project.Models.DTOs;
using http_project.Services;

using System.IO;
using System.Text.Json;
using System.Timers;

namespace http_project.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly VmsService _vmsService;

    [ObservableProperty]
    private bool _isServiceRunning;

    [ObservableProperty]
    private VMS _vms = new();

    private Thread? ShowingThread;
    private IEnumerable<Models.DTOs.RequestMessageDto> _messages;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public MainViewModel()
    {
        _vmsService = new();
        _vmsService.OnMessageReceived += _vmsService_OnMessageReceived;
    }

    private void Serialize()
    {
        var json = JsonSerializer.Serialize(_messages);
        File.WriteAllText("vms.json", json);
    }

    private void Deserialize()
    {
        if (File.Exists("vms.json"))
        {
            var json = File.ReadAllText("vms.json");
            _messages = JsonSerializer.Deserialize<IEnumerable<RequestMessageDto>>(json)
                ?? [];

            _vmsService_OnMessageReceived(this, _messages);
        }
    }

    private void _vmsService_OnMessageReceived(object? sender, IEnumerable<Models.DTOs.RequestMessageDto> e)
    {
        var count = e.Count();

        _messages = e;

        Serialize();

        ShowingThread = new Thread((object cancellationTokenObj) =>
        {
            var cancellationToken = (CancellationToken)cancellationTokenObj;

            while (!cancellationToken.IsCancellationRequested && count > 0)
            {
                Vms.Message = _messages.ElementAt(count - 1).Message;
                Vms.Pictorama = _messages.ElementAt(count - 1).Pictorama.ToString();
                Vms.Status = _messages.ElementAt(count - 1).Status.ToString();

                OnPropertyChanged(nameof(Vms));

                System.Threading.Thread.Sleep(5000);

                count--;

                if (count == 0) count = _messages.Count();

                if (cancellationToken.IsCancellationRequested) break;
            }
        })
        {
            IsBackground = true
        };

        if (!ShowingThread.IsAlive) ShowingThread.Start(_cancellationTokenSource.Token);
    }

    [RelayCommand]
    public void StartService()
    {
        _vmsService.Start();

        Deserialize();

        IsServiceRunning = true;
    }

    [RelayCommand]
    public void StopService()
    {
        _vmsService.Stop();
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new();
        Vms = new();
        IsServiceRunning = false;
    }
}
