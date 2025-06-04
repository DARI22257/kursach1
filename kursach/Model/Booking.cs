using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
     public class Booking
    {
        public int Id { get; set; }
        public int Numberroom { get; set; }
        public DateTime Datestart { get; set; }
        public DateTime Dateend { get; set; }
        public string Status { get; set; }
        public int GuestId { get; set; }
    }
}
