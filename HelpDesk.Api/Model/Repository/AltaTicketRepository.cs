using HelpDesk.Api.Entities.Models;
using HelpDesk.Api.Model.Context;
using HelpDesk.Api.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Model.Repository
{
    public class AltaTicketRepository : IAltaTicket
    {
        private readonly HelpDeskContext _context;

        public AltaTicketRepository(HelpDeskContext context)
        {
            _context = context;
        }

        public void Alta<T>(T ticket) where T : class
        {
            _context.Add(ticket);
        }

        public void Update<T>(T ticket) where T : class
        {            
            _context.Entry(ticket).State = EntityState.Modified;
        }

        public Task<Ticket> EditarTicketAlta(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public void Eliminar<T>(T ticket) where T : class
        {
            _context.Remove(ticket);
        }

        public  async Task<bool> GuardarCambiosAsync()
        {
            var guardado = await this._context.SaveChangesAsync();

            return guardado > 0;
        }

        public async Task<Planta[]> ObtenerPlantas()
        {
            IQueryable<Planta> query = (from p in _context.Planta
                                        select p).OrderBy(p => p.PlantaNombre);

            return await query.ToArrayAsync();
        }

        public async Task<Ticket> ObtenerTicketPorId(int ticketId)
        {
            IQueryable<Ticket> query = (from t in _context.Ticket
                                        join p in _context.Planta on t.PlantaId equals p.PlantaId
                                        where t.TicketId == ticketId
                                        select  new Ticket
                                        {
                                            PlantaId = t.PlantaId,
                                            AsignadoA = t.AsignadoA,
                                            ComentarioAlta = t.ComentarioAlta,
                                            ComentarioAsignado = t.ComentarioAsignado,
                                            EstadoTicket = t.EstadoTicket,
                                            FechaAlta = t.FechaAlta,
                                            FechaAsignado = t.FechaAsignado,
                                            FechaTermino = t.FechaTermino,
                                            NombreReporta = t.NombreReporta,
                                            Planta = p.PlantaNombre,
                                            TicketId = t.TicketId,
                                            TipoReporte = t.TipoReporte,
                                            ImagenUrl = t.ImagenUrl

                                        });

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Ticket[]> ObtenerTickets()
        {
            IQueryable<Ticket> query = (from t in _context.Ticket
                                        join p in _context.Planta on t.PlantaId equals p.PlantaId
                                        select new Ticket
                                        {
                                            PlantaId = t.PlantaId,
                                            AsignadoA = t.AsignadoA,
                                            ComentarioAlta = t.ComentarioAlta,
                                            ComentarioAsignado = t.ComentarioAsignado,
                                            EstadoTicket = t.EstadoTicket,
                                            FechaAlta = t.FechaAlta,
                                            FechaAsignado = t.FechaAsignado,
                                            FechaTermino = t.FechaTermino,
                                            NombreReporta = t.NombreReporta,
                                            Planta = p.PlantaNombre,
                                            TicketId = t.TicketId,
                                            TipoReporte = t.TipoReporte

                                        });

            return await query.ToArrayAsync();
        }
    }
}
