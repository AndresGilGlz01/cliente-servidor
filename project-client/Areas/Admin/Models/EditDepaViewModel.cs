namespace project_client.Areas.Admin.Models
{
    public class EditDepaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdSuperior { get; set; }
    }
}
