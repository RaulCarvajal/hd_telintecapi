using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Dtos
{
    public class TicketResponsableDto
    {
        public int Id { get; set; }
        public string AsignadoA { get; set; }
        public string Errores { get; set; } = String.Empty;
    }
}
