namespace tcp_wpf_server.Models.DTOs;

public class MessageDto
{
    public string Mensaje { get; set; } = null!;
    public string Origen { get; set; } = null!;
    public DateTime Fecha { get; set; }
}
