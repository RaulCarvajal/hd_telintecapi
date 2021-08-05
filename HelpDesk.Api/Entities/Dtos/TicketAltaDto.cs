using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Dtos
{
    public class TicketAltaDto
    {
        public int TipoReporte { get; set; }
        public string NombreReporta { get; set; }
        public string ComentarioAlta { get; set; }
        public string Errores { get; set; } = String.Empty;
        public int PlantaId { get; set; }
        public string Imagen { get; set; } 
        public string ImagenData { get; set; }
    }

    public class TicketAltaFileDto
    {
        public int TicketId { get; set; }
        public string TicketAlta { get; set; }
        [JsonIgnore]
        public IFormFile File { get; set; }

        public string Errores { get; set; } = String.Empty;
    }
}
