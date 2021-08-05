using AutoMapper;
using HelpDesk.Api.Entities.Dtos;
using HelpDesk.Api.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            this.CreateMap<TicketAltaDto, Ticket>();
            this.CreateMap<Ticket, TicketAltaDto>();
            this.CreateMap<TicketUpdateDto, Ticket>();
            this.CreateMap<TicketResponsableDto, Ticket>();
            this.CreateMap<TicketCerrarDto, Ticket>();
        }
    }
}
