namespace project_client.Areas.Usuario.Models
{
    public class AgregarDepartamentoViewModel
    {
        public string Nombre { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdSuperior { get; set; }
    }
}
