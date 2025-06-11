using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class BookMem
    {
        public int BookingId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestPhone { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string Datestart { get; set; }
        public string Dateend { get; set; }
        public string Status { get; set; }
        public int TotalPrice { get; set; }
    }
}
