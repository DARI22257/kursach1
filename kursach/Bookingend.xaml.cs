
using kursach.Model;
using kursachModel;
using System;
using System.Windows;

namespace kursach
{
    public partial class Bookingend : Window
    {
        private Booking booking;
        private Guest guest;
        private NumberModel room;

        public Bookingend(Booking booking, Guest guest, NumberModel room)
        {
            InitializeComponent();
            this.booking = booking ?? throw new ArgumentNullException(nameof(booking));
            this.guest = guest ?? throw new ArgumentNullException(nameof(guest));
            this.room = room ?? throw new ArgumentNullException(nameof(room));

            GuestNameText.Text = $"{guest.FirstName} {guest.Surname} {guest.Lastname}";
            PhoneText.Text = guest.Phone;
            RoomNumberText.Text = room.Numberroom.ToString();
            RoomTypeText.Text = room.Type;
            BookingDatesText.Text = $"{booking.Datestart:dd.MM.yyyy} - {booking.Dateend:dd.MM.yyyy}";
            StatusText.Text = booking.Status;
        }

        private void ConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на null
            if (guest == null || room == null)
            {
                MessageBox.Show("Ошибка: не выбран гость или номер.");
                return;
            }

            // Проверка существования в БД
            var validGuest = GuestDB.GetDb().SelectAll().FirstOrDefault(g => g.Id == guest.Id);
            var validRoom = NumberDB.GetDb().SelectAll().FirstOrDefault(r => r.Id == room.Id);

            if (validGuest == null)
            {
                MessageBox.Show("Ошибка: гость не найден в базе данных.");
                return;
            }

            if (validRoom == null)
            {
                MessageBox.Show("Ошибка: номер не найден в базе данных.");
                return;
            }

            // Присваивание и вставка
            booking.RoomId = room.Id;
            booking.GuestId = guest.Id;

            bool success = BookingDB.GetDb().Insert(booking);

            if (success)
            {
                MessageBox.Show("Бронь подтверждена.");
                BookingMemory memoryWindow = new BookingMemory();
                memoryWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении.");
            }
        }


        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}