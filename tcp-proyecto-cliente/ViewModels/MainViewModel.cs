using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Net;

using tcp_proyecto_cliente.Models.DTOs;
using tcp_proyecto_cliente.Services;

namespace tcp_proyecto_cliente.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PictureDto> _pictures = [];

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _ipAddress = string.Empty;

    [ObservableProperty]
    private int _port;

    [ObservableProperty]
    private bool _isConnected;

    private readonly GaleryService _galeryService = new();

    public MainViewModel()
    {
        _galeryService.OnSendMessage += OnSendMessage;
        _galeryService.OnConnectServer += OnConnectServer;
        _galeryService.OnReceiveMessage += OnReceiveMessage;
        _galeryService.OnDisconnectServer += OnDisconnectServer;
        _galeryService.OnDeniedServer += OnDeniedServer;
        _galeryService.OnUnexpected += OnUnexpected;

        Username = Environment.UserName;
        IpAddress = "127.0.0.1";
        Port = 9630;
    }

    [RelayCommand]
    private void Connect()
    {
        var ipAddress = IPAddress.Parse(IpAddress);

        IsConnected = true;
        //_galeryService.Connect(ipAddress, Port);
    }

    private void OnUnexpected(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDeniedServer(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnSendMessage(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnReceiveMessage(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDisconnectServer(object? sender, EventArgs e)
    {
        IsConnected = false;
    }

    private void OnConnectServer(object? sender, EventArgs e)
    {
        IsConnected = true;
    }
}
