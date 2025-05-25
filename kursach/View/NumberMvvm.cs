using kursach.Model;
using kursachModel;
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
        private kursachModel.NumberModel newNumber = new();

        public kursachModel.NumberModel NewNumber
        {
            get => newNumber;
            set
            {
                newNumber = value;
                Signal();
            }
        }
        private NumberModel selectedNumberModel = new NumberModel();
        public NumberModel SelectedNumberModel
        {
            get => selectedNumberModel;
            set
            {
                selectedNumberModel = value;
                Signal();
            }
        }

        private ObservableCollection<NumberModel> number ;

        public ObservableCollection<NumberModel> Number
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
            Number = new ObservableCollection<NumberModel>(NumberDB.GetDb().SelectAll().Select(s => (NumberModel)s));
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
