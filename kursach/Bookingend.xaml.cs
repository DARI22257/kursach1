using kursach.View;
using kursachModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для Bookingend.xaml
    /// </summary>
    public partial class Bookingend : Window
    {
        private Booking booking;
        private Guest guest;
        private NumberModel room;
        public Bookingend(Booking booking, Guest guest, NumberModel room)
        {
            InitializeComponent();

            GuestNameText.Text = $"{guest.FirstName} {guest.Surname} {guest.Lastname}";
            PhoneText.Text = guest.Phone;
            RoomNumberText.Text = room.Numberroom.ToString();
            RoomTypeText.Text = room.Type;
            BookingDatesText.Text = $"{booking.Datestart:dd.MM.yyyy} - {booking.Dateend:dd.MM.yyyy}";
            StatusText.Text = booking.Status;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save booking to database
            booking.Numberroom = room.Numberroom;
            booking.GuestId = guest.Id;

            bool success = BookingDB.GetDb().Insert(booking);

            if (success)
                MessageBox.Show("Бронь успешно сохранена.");
            else
                MessageBox.Show("Ошибка при сохранении брони.");
        }
    }
}
