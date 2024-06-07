namespace project_signalr_administrador.Models.DTOs.Request;

public class UpdateTurnoRequest
{
    public int IdTurno { get; set; }
    public int IdCaja { get; set; }
    public string? Estado { get; set; }
}
