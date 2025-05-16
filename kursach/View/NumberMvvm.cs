using kursach.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.View
{
    internal class NumberMvvm : BaseVM
    {
        private kursachModel.Number newNumber = new();

        public kursachModel.Number NewNumber
        {
            get => newNumber;
            set
            {
                newNumber = value;
                Signal();
            }
        }

        public CommandMvvm InsertClient { get; set; }
        public NumberMvvm()
        {
            InsertClient = new CommandMvvm(() =>
            {
                NumberDB.GetDb().Insert(NewNumber);
                close?.Invoke();
            },
                () =>
                newNumber.Numberroom != 0 &&
                !string.IsNullOrEmpty(newNumber.Type) &&
                !string.IsNullOrEmpty(newNumber.Status) &&
                newNumber.Price != 0);
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
