namespace tcp_proyecto_cliente.Models.DTOs;

public class PictureDto
{
    public string Autor { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Image { get; set; } = null!;
    public DateTime Date { get; set; }
}
