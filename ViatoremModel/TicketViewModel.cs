using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViatoremModel
{
    public class TicketViewModel
    {
        public User user { get; set; }
        public IEnumerable<Seat> seats { get; set; }
    }
}
