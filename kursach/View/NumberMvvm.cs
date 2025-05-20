using kursach.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Number> number ;

        public ObservableCollection<Number> Number
        {

            get => number;
            set
            {
                number = value;
                Signal();
            }
        }
        public CommandMvvm InsertNumber { get; set; }
        public NumberMvvm()
        {
            Number = new ObservableCollection<Number>(NumberDB.GetDb().SelectAll());
            InsertNumber = new CommandMvvm(() =>
            {
                NumberDB.GetDb().Insert(newNumber); 
                Number.Add(NewNumber); 
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
