namespace project_client.Areas.Admin.Models
{
    public class AddActDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int IdDepartamento { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
        public int Estado {  get; set; }
        public string Imagen { get; set; }
    }
}
