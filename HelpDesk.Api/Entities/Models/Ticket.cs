using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Models
{
    [Table("ticket")]
    public class Ticket
    {
        [Column("ticked_id")]
        public int TicketId { get; set; }  
        [Column("planta_id")]
        public int PlantaId { get; set; } 
        [Required]
        public int TipoReporte { get; set; }
        [Required]
        public int EstadoTicket { get; set; }
        public string NombreReporta { get; set; }
        public string ComentarioAlta { get; set; }
        public string AsignadoA { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaAsignado { get; set; }
        public DateTime FechaTermino { get; set; }
        public string ComentarioAsignado { get; set; }

        public string  ImagenUrl { get; set; }

        [NotMapped]
        public string Planta { get; set; }
    }
}
