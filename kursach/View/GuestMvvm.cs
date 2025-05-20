using kursach.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ObservableCollection<Guest> Guest
        {
            get => guest;
            set
            {
                guest = value;
                Signal();
            }
        }

        public CommandMvvm InsertGuest { get; set; }
        public GuestMvvm()
        {
            Guest = new ObservableCollection<Guest>(GuestDB.GetDb().SelectAll());
            InsertGuest = new CommandMvvm(() =>
            {
                GuestDB.GetDb().Insert(NewGuest);
                Guest.Add(NewGuest);
                close?.Invoke();
            },
                () =>
                !string.IsNullOrEmpty(newGuest.FirstName) &&
                !string.IsNullOrEmpty(newGuest.Surname) &&
                !string.IsNullOrEmpty(newGuest.Lastname) &&
                !string.IsNullOrEmpty(newGuest.Phone) &&
                !string.IsNullOrEmpty(newGuest.Email) &&
                !string.IsNullOrEmpty(newGuest.Passportdata));

        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }

    }
}
