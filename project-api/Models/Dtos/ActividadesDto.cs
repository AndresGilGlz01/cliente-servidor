﻿namespace project_api.Models.Dtos
{
    public class ActividadesDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int IdDepartamento { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public int Estado { get; set; }
        public string Departamento { get;set; }=null!;
    }
}
