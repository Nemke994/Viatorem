using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViatoremModel
{
    public class HomeViewModel
    {
        public User user { get; set; }
        public IEnumerable<Location> locations { get; set; }
        public IEnumerable<Schedule> schedules { get; set; }
        public IEnumerable<Seat> seats { get; set; }

    }
}
