using System;
using System.Collections.Generic;
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

        public CommandMvvm InsertClient { get; set; }
        public BookingMvvm()
        {
            InsertClient = new CommandMvvm(() =>
            {
                BookingDB.GetDb().Insert(NewBooking);
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
