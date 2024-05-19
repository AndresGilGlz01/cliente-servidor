namespace project_client.Areas.Usuario.Models
{
    public class AgregarActividadViewModel
    {
        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int? IdDepartamento { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
        public List<Departamentos> Departamentos { get; set; } = null!;
        public IFormFile? Archivo { get; set; }
    }
    public class Departamentos
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? IdSuperior { get; set; }
        public string? DepartamentoSuperior { get; set; }
    }
}
