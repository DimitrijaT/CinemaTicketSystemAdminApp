using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTicketSystemAdminApp.Models
{
    public class Order
    {

        public Guid Id { get; set; } // Because we don't inherit BaseEntity here, we just add it manually
        public string UserId { get; set; }

        public CinemaTicketSystemUser User { get; set; }

        public IEnumerable<TicketOrder> TicketOrders { get; set; }
    }
}
