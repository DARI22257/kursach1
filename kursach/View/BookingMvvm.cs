using kursach.Model;
using kursachModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace kursach.View
{
    internal class BookingMvvm : BaseVM
    {
        public Booking NewBooking { get; set; }

        public ObservableCollection<Guest> Guests { get; set; }
        public ObservableCollection<NumberModel> Number { get; set; }
        public ObservableCollection<Booking> Booking { get; set; }

        public Guest SelectedGuest { get; set; }
        public NumberModel SelectedRoom { get; set; }

        public CommandMvvm InsertBooking { get; set; }

        private Action close;
        internal void SetClose(Action close) => this.close = close;

        public BookingMvvm()
        {
            Guests = new ObservableCollection<Guest>(GuestDB.GetDb().SelectAll());
            Number = new ObservableCollection<NumberModel>(
                NumberDB.GetDb().SelectAll().Where(r => r.Status == "Свободен"));
            Booking = new ObservableCollection<Booking>(BookingDB.GetDb().SelectAll());

            NewBooking = new Booking
            {
                Status = "",
                Datestart = DateTime.Today,
                Dateend = DateTime.Today
            };

            InsertBooking = new CommandMvvm(() =>
            {
                NewBooking.GuestId = SelectedGuest?.Id ?? 0;
                NewBooking.RoomId = SelectedRoom?.Id ?? 0;

                if (BookingDB.GetDb().Insert(NewBooking))
                {
                    // Обновить статус номера
                    SelectedRoom.Status = "Занят";
                    NumberDB.GetDb().Update(SelectedRoom);

                    Booking.Add(NewBooking);
                    NewBooking = new Booking
                    {
                        Status = "",
                        Datestart = DateTime.Today,
                        Dateend = DateTime.Today
                    };
                    Signal(nameof(NewBooking));
                    close?.Invoke();
                }
                else
                {
                    System.Windows.MessageBox.Show("Ошибка при сохранении.");
                }
            },
            () =>
                NewBooking != null &&
                NewBooking.Datestart != DateTime.MinValue &&
                NewBooking.Dateend != DateTime.MinValue &&
                !string.IsNullOrWhiteSpace(NewBooking.Status) &&
                SelectedGuest != null &&
                SelectedRoom != null);
        }
    }
}
