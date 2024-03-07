using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Net;

using tcp_wpf_server.Models.DTOs;
using tcp_wpf_server.Services;

namespace tcp_wpf_server.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ChatServer Server { get; set; } = new();

    [ObservableProperty]
    private ObservableCollection<string> _users = [];

    [ObservableProperty]
    private string _ipAddress = "0.0.0.0";

    [ObservableProperty]
    private ObservableCollection<MessageDto> _messages = [];

    [ObservableProperty]
    private int _messageNumber;

    public MainViewModel()
    {
        var addresses = Dns.GetHostAddresses(Dns.GetHostName());

        _ipAddress = string.Join(",", addresses.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x => x.ToString()));

        Server.OnReceivedMessage += Server_OnReceivedMessage;
    }

    private void Server_OnReceivedMessage(object? sender, MessageDto e)
    {
        if (e.Mensaje == "**HELLO")
        {
            e.Mensaje = $"{e.Origen} se ha conectado";
            Users.Add(e.Origen);
        }
        else if (e.Mensaje == "**BYE")
        {
            e.Mensaje = $"{e.Origen} se ha desconectado";
            Users.Remove(e.Origen);
        }

        Messages.Add(e);
        MessageNumber = Messages.Count - 1;
    }

    [RelayCommand]
    public void Detener()
    {
        Messages.Clear();
        Users.Clear();

        Server.Stop();
    }

    [RelayCommand]
    public void StartServer() => Server.Start();
}
