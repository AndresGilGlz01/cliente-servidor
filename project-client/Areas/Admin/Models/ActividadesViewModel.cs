namespace project_client.Areas.Admin.Models
{
    public class ActividadesViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int? IdDepartamento { get; set; }
        public DateOnly? FechaRealizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }
        public string Departamento { get; set; }
    }
}
