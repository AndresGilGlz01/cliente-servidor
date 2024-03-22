namespace htttp_ejercicio_1.Models;

public class PostIt
{
    public string Titulo { get; set; } = null!;
    public string Contenido { get; set; } = null!;
    public string Remitente { get; set; } = null!;
    public double Posicionx { get; set; }
    public double Posiciony { get; set; }
    public double Angulo { get; set; }
}
