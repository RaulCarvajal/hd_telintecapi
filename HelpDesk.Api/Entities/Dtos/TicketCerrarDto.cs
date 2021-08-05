using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Dtos
{
    public class TicketCerrarDto
    {
        public int Id { get; set; }
        public string ComentarioAsignado { get; set; }
        public bool CerrarTicket { get; set; }
        public string Errores { get; set; } = string.Empty;
    }
}
