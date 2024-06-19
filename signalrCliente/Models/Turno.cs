namespace signalrCliente.Models
{
    public class Turno
    {
        public int Id { get; set; }

        public string Folio { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public DateTime Fecha { get; set; }

       
    }
}
