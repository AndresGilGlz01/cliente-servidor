namespace project_client.Areas.Usuario.Models
{
    public class GetActividadViewModel
    {
        public Actividad? Actividad { get; set; }

    }
    public class Actividad
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public int? IdDepartamento { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile? Archivo { get; set; }
    }
}
