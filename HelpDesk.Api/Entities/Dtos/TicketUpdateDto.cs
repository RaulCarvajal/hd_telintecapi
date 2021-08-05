using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Dtos
{
    public class TicketUpdateDto
    {
        public int Id { get; set; }
        public int PlantaId { get; set; }
        public int TipoReporte { get; set; }
        public string NombreReporta { get; set; }
        public string ComentarioAlta { get; set; }
        public string Errores { get; set; }
    }

    public class TicketUpdateFileDto
    {
        public string TicketUpdate { get; set; }
        [JsonIgnore]
        public IFormFile File { get; set; }

        public string Errores { get; set; } = String.Empty;
    }
}
