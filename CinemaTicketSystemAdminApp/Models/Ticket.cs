using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTicketSystemAdminApp.Models
{
    public class Ticket
    {

        public Guid Id { get; set; } // Because we don't inherit BaseEntity here, we just add it manually
        public string MovieName { get; set; }

        public string ImageUrl { get; set; }

        public string Genre { get; set; }

        public DateTime Date { get; set; }
        public int Price { get; set; }
    }
}
