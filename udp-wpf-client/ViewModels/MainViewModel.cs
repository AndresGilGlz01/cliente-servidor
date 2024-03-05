using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using udp_wpf_client.Models.DTOs;
using udp_wpf_client.Services;

namespace udp_wpf_client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private InscribeDto _inscribe = new();

    [ObservableProperty]
    private string _ipAddress = "0.0.0.0";

    private readonly WorkshopService service = new();

    [RelayCommand]
    private void Register()
    {
        service.IpAddress = IpAddress;

        service.SendRequest(Inscribe);
    }
}
