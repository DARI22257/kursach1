using kursach.Model;
using System;
using System.Collections.ObjectModel;

namespace kursach.View
{
    internal class GuestMvvm : BaseVM
    {
        private Guest newGuest = new();
        public Guest NewGuest
        {
            get => newGuest;
            set
            {
                newGuest = value;
                Signal();
            }
        }

        private ObservableCollection<Guest> guest;
        private Guest selectedGuest = new();
        public Guest SelectedGuest
        {
            get => selectedGuest;
            set
            {
                selectedGuest = value;
                if (value != null)
                {
                    NewGuest = new Guest
                    {
                        Id = value.Id,
                        FirstName = value.FirstName,
                        Surname = value.Surname,
                        Lastname = value.Lastname,
                        Phone = value.Phone,
                        Email = value.Email,
                        Passportdata = value.Passportdata
                    };
                }
                Signal();
            }
        }

        public ObservableCollection<Guest> Guests
        {
            get => guest;
            set
            {
                guest = value;
                Signal();
            }
        }

        public CommandMvvm InsertGuest { get; set; }
        public CommandMvvm UpdateGuest { get; set; }

        public GuestMvvm()
        {
            try
            {
                Guests = new ObservableCollection<Guest>(GuestDB.GetDb().SelectAll());
            }
            catch
            {
                Guests = new ObservableCollection<Guest>();
            }

            InsertGuest = new CommandMvvm(() =>
            {
                GuestDB.GetDb().Insert(NewGuest);
                Guests.Add(NewGuest);
                NewGuest = new();
                Signal(nameof(NewGuest));
                close?.Invoke();
            },
            () =>
                !string.IsNullOrEmpty(NewGuest.FirstName) &&
                !string.IsNullOrEmpty(NewGuest.Surname) &&
                !string.IsNullOrEmpty(NewGuest.Lastname) &&
                !string.IsNullOrEmpty(NewGuest.Phone) &&
                !string.IsNullOrEmpty(NewGuest.Email) &&
                !string.IsNullOrEmpty(NewGuest.Passportdata));

            UpdateGuest = new CommandMvvm(() =>
            {
                GuestDB.GetDb().Update(NewGuest);
                var index = Guests.IndexOf(SelectedGuest);
                Guests[index] = NewGuest;
                SelectedGuest = null;
                NewGuest = new Guest();
                Signal(nameof(NewGuest));
            },
            () => SelectedGuest != null &&
                  !string.IsNullOrEmpty(NewGuest.FirstName) &&
                  !string.IsNullOrEmpty(NewGuest.Surname));
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}