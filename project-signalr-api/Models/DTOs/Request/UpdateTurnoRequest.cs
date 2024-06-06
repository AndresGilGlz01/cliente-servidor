namespace project_signalr_api.Models.DTOs.Request;

public class UpdateTurnoRequest
{
    public int IdTurno { get; set; }
    public int IdCaja { get; set; }
    public string? Estado { get; set; }
}
