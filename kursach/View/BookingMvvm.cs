using kursach.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.View
{
    internal class BookingMvvm : BaseVM
    {
        private Booking newBooking = new();

        public Booking NewBooking
        {
            get => newBooking;
            set
            {
                newBooking = value; 
                Signal();
            }
        }
        private ObservableCollection<Booking> booking = new();

        public ObservableCollection<Booking> Booking
        {
            get => booking;
            set
            {
                booking = value;
                Signal();
            }
        }
        public CommandMvvm InsertBooking { get; set; }
        public BookingMvvm()
        {
            Booking = new ObservableCollection<Booking>(BookingDB.GetDb().SelectAll());
            InsertBooking = new CommandMvvm(() =>
            {
                BookingDB.GetDb().Insert(NewBooking);
                Booking.Add(NewBooking);
                close?.Invoke();
            },
                () =>
                newBooking.Numberroom != 0 &&
                DateTime.MinValue != newBooking.Datestart &&
                DateTime.MinValue != newBooking.Dateend &&
                !string.IsNullOrEmpty(newBooking.Status));
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
