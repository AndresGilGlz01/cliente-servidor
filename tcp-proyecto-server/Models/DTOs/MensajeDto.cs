using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_proyecto_server.Models.DTOs
{
    public class MensajeDto
    {
        public string Name { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
