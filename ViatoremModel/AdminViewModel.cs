using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViatoremModel
{
    public class AdminViewModel
    {
        public IEnumerable<Location> locations { get; set; }
        public IEnumerable<Route> routes { get; set; }
        public IEnumerable<Bus> buses { get; set; }
        public IEnumerable<Schedule> schedules { get; set; }
        public IEnumerable<User> users { get; set; }
    }
}
