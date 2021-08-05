using HelpDesk.Api.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Model.Interfaces
{
    public interface IAltaTicket
    {
        void Alta<T>(T ticket) where T : class;
        void Eliminar<T>(T ticket) where T : class;
        void Update<T>(T ticket) where T : class;
        Task<Ticket> EditarTicketAlta(Ticket ticket);
        Task<Ticket> ObtenerTicketPorId(int ticketId);
        Task<bool> GuardarCambiosAsync();
        Task<Ticket[]> ObtenerTickets();
        Task<Planta[]> ObtenerPlantas();

    }
}
