using kursach.Model;
using kursach.View;
using kursachModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace kursach
{
    public partial class BookingMemory : Window
    {
        public ObservableCollection<BookMem> Bookings { get; set; }

        public BookingMemory()
        {
            InitializeComponent();
            DataContext = new BookingMemoryMvvm();


            var rawBookings = BookingDB.GetDb().SelectAll();
            var guests = GuestDB.GetDb().SelectAll();
            var rooms = NumberDB.GetDb().SelectAll();

            Bookings = new ObservableCollection<BookMem>(
                from booking in rawBookings
                join guest in guests on booking.GuestId equals guest.Id
                join room in rooms on booking.RoomId equals room.Id
                where booking.Datestart > DateTime.MinValue
                select new BookMem
                {
                    GuestFullName = $"{guest.FirstName} {guest.Surname} {guest.Lastname}",
                    GuestPhone = guest.Phone,
                    RoomNumber = room.Numberroom,
                    RoomType = room.Type,
                    Datestart = booking.Datestart.ToShortDateString(),
                    Dateend = booking.Dateend.ToShortDateString(),
                    Status = booking.Status
                });
        }
    }
}