namespace project_client.Areas.Admin.Models;

public class IndexViewModel
{
    public IEnumerable<ActividadModel> Actividades { get; set; } = [];
    public IEnumerable<DepartamentoModel> Departamentos { get; set; } = [];
    public string Token { get; set; } = null!;

    public class DepartamentoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class ActividadModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int? IdDepartamento { get; set; }
        public DateOnly? FechaRealizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }
        public string Departamento { get; set; } = null!;
    }
}