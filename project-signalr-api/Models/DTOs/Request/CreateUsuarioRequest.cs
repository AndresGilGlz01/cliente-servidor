namespace project_signalr_api.Models.DTOs.Request;

public class CreateUsuarioRequest
{
    public string? Nombre { get; set; }
    public string? Contraseña { get; set; }
    public string? ConfirmarContraseña { get; set; }
}