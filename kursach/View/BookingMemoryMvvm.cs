using kursach.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace kursach.View
{
    public class BookingMemoryMvvm : INotifyPropertyChanged
    {
        private ObservableCollection<BookMem> allBookings;
        private ObservableCollection<BookMem> bookings;

        public ObservableCollection<BookMem> Bookings
        {
            get => bookings;
            set
            {
                bookings = value;
                OnPropertyChanged();
            }
        }

        private string search;
        public string Search
        {
            get => search;
            set
            {
                if (search != value)
                {
                    search = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        private string selectedRoomType;
        public string SelectedRoomType
        {
            get => selectedRoomType;
            set
            {
                selectedRoomType = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string selectedStatus;
        public string SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public ObservableCollection<string> RoomTypes { get; set; }
        public ObservableCollection<string> Statuses { get; set; }
        private BookMem selectedBooking;
        public BookMem SelectedBooking
        {
            get => selectedBooking;
            set
            {
                selectedBooking = value;
                OnPropertyChanged();
                // Обновить доступность команды при изменении выбора
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public CommandMvvm DeleteBooking { get; set; }

        public BookingMemoryMvvm()
        {
            LoadData();

            DeleteBooking = new CommandMvvm(() =>
            {
                if (SelectedBooking == null)
                    return;

                var booking = BookingDB.GetDb().SelectAll()
                    .FirstOrDefault(b => b.Id == SelectedBooking.BookingId);

                if (booking != null && BookingDB.GetDb().Remove(booking))
                {
                    // 1. Найти номер по RoomId
                    var room = NumberDB.GetDb().SelectAll().FirstOrDefault(r => r.Id == booking.RoomId);

                    // 2. Обновить статус
                    if (room != null)
                    {
                        room.Status = "Свободен";
                        NumberDB.GetDb().Update(room);
                    }

                    // 3. Удалить из отображаемого списка
                    allBookings.Remove(SelectedBooking);
                    Bookings.Remove(SelectedBooking);
                    SelectedBooking = null;
                }
                else
                {
                    MessageBox.Show("Не удалось удалить бронь.");
                }
            },
            () => SelectedBooking != null); // ← Кнопка активна только при выборе
        }

        private void LoadData()
        {
            var rawBookings = BookingDB.GetDb().SelectAll();
            var guests = GuestDB.GetDb().SelectAll();
            var rooms = NumberDB.GetDb().SelectAll();

            allBookings = new ObservableCollection<BookMem>(
                from booking in rawBookings
                join guest in guests on booking.GuestId equals guest.Id
                join room in rooms on booking.RoomId equals room.Id
                select new BookMem
                {
                    BookingId = booking.Id,
                    GuestFullName = $"{guest.FirstName} {guest.Surname} {guest.Lastname}",
                    GuestPhone = guest.Phone,
                    RoomNumber = room.Numberroom,
                    RoomType = room.Type,
                    Datestart = booking.Datestart.ToShortDateString(),
                    Dateend = booking.Dateend.ToShortDateString(),
                    Status = booking.Status,
                    TotalPrice = room.Price * Math.Max(1, (booking.Dateend - booking.Datestart).Days)
                });

            Bookings = new ObservableCollection<BookMem>(allBookings);
            RoomTypes = new ObservableCollection<string>(allBookings.Select(b => b.RoomType).Distinct());
            Statuses = new ObservableCollection<string>(allBookings.Select(b => b.Status).Distinct());
        }

        private void ApplyFilters()
        {
            var filtered = allBookings.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                filtered = filtered.Where(b => b.GuestFullName.StartsWith(Search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedRoomType))
            {
                filtered = filtered.Where(b => b.RoomType == SelectedRoomType);
            }

            if (!string.IsNullOrWhiteSpace(SelectedStatus))
            {
                filtered = filtered.Where(b => b.Status == SelectedStatus);
            }

            Bookings = new ObservableCollection<BookMem>(filtered);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}