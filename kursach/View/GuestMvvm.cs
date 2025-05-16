using System;
using System.Collections.Generic;
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

        public CommandMvvm InsertClient { get; set; }
        public GuestMvvm()
        {
            InsertClient = new CommandMvvm(() =>
            {
                GuestDB.GetDb().Insert(NewGuest);
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
