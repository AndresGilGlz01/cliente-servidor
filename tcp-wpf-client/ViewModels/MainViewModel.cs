using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Threading;

using tcp_wpf_client.Models.DTOs;
using tcp_wpf_client.Services;

namespace tcp_wpf_client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MessageDto> _messages = [];

    [ObservableProperty]
    private string _message = string.Empty;

    [ObservableProperty]
    private string _ipAddress = "0.0.0.0";

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private int _messageNumber;

    private ChatService client = new();

    public MainViewModel()
    {
        client.OnReceiveMessage += Client_OnReceiveMessage;
    }

    [RelayCommand]
    private void Connect()
    {
        _ = IPAddress.TryParse(IpAddress, out IPAddress? ipAddress);

        if (ipAddress is null) return;

        client.Connect(ipAddress);

        IsConnected = true;
    }

    [RelayCommand]
    private void Send()
    {
        if (string.IsNullOrWhiteSpace(Message)) return;

        var message = new MessageDto()
        {
            Fecha = DateTime.Now,
            Origen = client.Equipo,
            Mensaje = Message
        };

        client.SendMessage(message);
    }

    private void Client_OnReceiveMessage(object? sender, MessageDto e)
    {
        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            if (e.Mensaje == "**HELLO")
            {
                e.Mensaje = $"{e.Origen} se ha conectado";
            }
            if (e.Mensaje == "**BYE")
            {
                e.Mensaje = $"{e.Origen} se ha desconectado";
            }

            Messages.Add(e);

            MessageNumber = Messages.Count - 1;
        });
    }
}
