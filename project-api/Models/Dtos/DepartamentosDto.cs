namespace project_api.Models.Dtos
{
    public class DepartamentosDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? IdSuperior { get; set; }
        public string? DepartamentoSuperior { get; set; }
    }
}
