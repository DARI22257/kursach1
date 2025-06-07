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
        public int RoomId { get; set; }
        public System.DateTime Datestart { get; set; } = DateTime.Now;
        public System.DateTime Dateend { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public int GuestId { get; set; }
    }
}
