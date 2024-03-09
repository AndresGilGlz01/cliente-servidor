using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tcp_proyecto_server.Models.DTOs
{
    public class PictureDto
    {
        public string Autor { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Image { get; set; } = null!;
        public BitmapImage? img { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
