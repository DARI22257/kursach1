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

            // Загружаем данные
            var rawBookings = BookingDB.GetDb().SelectAll();
            var guests = GuestDB.GetDb().SelectAll();
            var rooms = NumberDB.GetDb().SelectAll();

            AutoCheckoutRooms(rawBookings, rooms);

            // Формируем отображаемые записи
            Bookings = new ObservableCollection<BookMem>(
                from booking in rawBookings
                join guest in guests on booking.GuestId equals guest.Id
                join room in rooms on booking.RoomId equals room.Id
                where booking.Datestart > DateTime.MinValue
                select new BookMem
                {
                    BookingId = booking.Id,
                    GuestFullName = $"{guest.FirstName} {guest.Surname} {guest.Lastname}",
                    GuestPhone = guest.Phone,
                    RoomNumber = room.Numberroom,
                    RoomType = room.Type,
                    Datestart = booking.Datestart.ToShortDateString(),
                    Dateend = booking.Dateend.ToShortDateString(),
                    Status = booking.Status
                });
        }

        private void AutoCheckoutRooms(IEnumerable<Booking> bookings, IEnumerable<NumberModel> rooms)
        {
            foreach (var booking in bookings)
            {
                if (booking.Dateend < DateTime.Today)
                {
                    var room = rooms.FirstOrDefault(r => r.Id == booking.RoomId);
                    if (room != null && room.Status != "Свободен")
                    {
                        room.Status = "Свободен";
                        NumberDB.GetDb().Update(room);
                    }
                }
            }
        }
    }
}