using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using System.Collections.ObjectModel;
using System.IO;
using System.Net;

using tcp_proyecto_cliente.Models.DTOs;
using tcp_proyecto_cliente.Services;

namespace tcp_proyecto_cliente.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public event EventHandler? OnSelectedPicture;
    public event EventHandler? OnSendPicture;
    public event EventHandler? OnCancel;

    [ObservableProperty]
    private ObservableCollection<PictureDto> _pictures = [];

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private PictureDto? _selectedPicture;

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

    [RelayCommand]
    private void Disconnect()
    {
        IsConnected = false;
        //_galeryService.Disconnect();
        SelectedPicture = null;
        Pictures.Clear();
        OnPropertyChanged(nameof(Pictures));
    }

    [RelayCommand]
    private void ChoosePhoto()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif, *.bmp, *.dib, *.tif, *.tiff, *.ico, *.icon, *.svg, *.svgz, *.webp)|*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.gif;*.bmp;*.dib;*.tif;*.tiff;*.ico;*.icon;*.svg;*.svgz;*.webp"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var picture = new
            {
                Image = File.ReadAllBytes(openFileDialog.FileName),
                Date = DateTime.Now
            };

            SelectedPicture = new PictureDto
            {
                Autor = Username,
                Image = Convert.ToBase64String(picture.Image),
                Date = picture.Date
            };

            OnSelectedPicture?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private void SendPhoto()
    {
        if (SelectedPicture is not null)
        {
            SelectedPicture.Subject = "Add";
            _galeryService.SendMessage(SelectedPicture); // Send a message to the server to remove the picture

            Pictures.Add(SelectedPicture);
            SelectedPicture = null;

            OnSendPicture?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        SelectedPicture = null;

        OnCancel?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void RemovePhoto(PictureDto picture)
    {
        Pictures.Remove(picture);
        OnPropertyChanged(nameof(Pictures));
        
        picture.Subject = "Remove";
        _galeryService.SendMessage(picture); // Send a message to the server to remove the picture
    }

    private void OnUnexpected(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDeniedServer(object? sender, EventArgs e)
    {
        IsConnected = false;
    }

    private void OnSendMessage(object? sender, PictureDto e)
    {
        Pictures.Add(e);

        OnPropertyChanged(nameof(Pictures));
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
