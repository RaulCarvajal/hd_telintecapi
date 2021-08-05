using HelpDesk.Api.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Model.Context
{
    public class HelpDeskContext : DbContext
    { 
        public HelpDeskContext(DbContextOptions<HelpDeskContext> opts) : base(opts)
        {
        }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Planta> Planta { get; set; }
    }
}
