using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Entities.Models
{
    [Table("planta")]
    public class Planta
    {
        [Column("planta_id")]
        public int PlantaId { get; set; }

        [Column("planta_nombre")]
        public string PlantaNombre { get; set; }
    }
}
