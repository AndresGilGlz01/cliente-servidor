namespace project_signalr_api.Models.DTOs.Request;

public class CreateUsuarioRequest
{
    public string? Nombre { get; set; }
    public string? Contrasena { get; set; }
    public string? ConfirmarContrasena { get; set; }
}