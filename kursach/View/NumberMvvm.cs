using kursach.Model;
using kursachModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace kursach.View
{
    internal class NumberMvvm : BaseVM
    {
        private NumberModel newNumber = new();

        public NumberModel NewNumber
        {
            get => newNumber;
            set
            {
                newNumber = value;
                Signal();
            }
        }

        private NumberModel selectedNumberModel = new();
        public NumberModel SelectedNumberModel
        {
            get => selectedNumberModel;
            set
            {
                selectedNumberModel = value;
                if (value != null)
                {
                    NewNumber = new NumberModel
                    {
                        Id = value.Id,
                        Numberroom = value.Numberroom,
                        Type = value.Type,
                        Status = value.Status,
                        Price = value.Price
                    };
                }
                Signal();
            }
        }

        private ObservableCollection<NumberModel> number;
        public ObservableCollection<NumberModel> Number
        {
            get => number;
            set
            {
                number = value;
                Signal();
            }
        }

        // Коллекции для ComboBox
        public ObservableCollection<string> RoomTypes { get; set; } = new()
        {
            "Люкс", "Стандарт", "Президентский"
        };

        public ObservableCollection<string> StatusOptions { get; set; } = new()
        {
            "Свободен", "Занят"
        };

        public CommandMvvm InsertNumber { get; set; }
        public CommandMvvm UpdateNumber { get; set; }
        public CommandMvvm RemoveNumber { get; set; }

        public NumberMvvm()
        {
            try
            {
                Number = new ObservableCollection<NumberModel>(NumberDB.GetDb().SelectAll());
            }
            catch
            {
                Number = new ObservableCollection<NumberModel>();
            }

            InsertNumber = new CommandMvvm(() =>
            {
                if (NumberDB.GetDb().Insert(NewNumber))
                {
                    Number.Add(NewNumber);
                    NewNumber = new NumberModel();
                    Signal(nameof(NewNumber));
                    close?.Invoke();
                }
            },
            () =>
                NewNumber.Numberroom > 0 &&
                !string.IsNullOrWhiteSpace(NewNumber.Type) &&
                !string.IsNullOrWhiteSpace(NewNumber.Status) &&
                NewNumber.Price > 0);

            UpdateNumber = new CommandMvvm(() =>
            {
                if (NumberDB.GetDb().Update(NewNumber))
                {
                    var idx = Number.IndexOf(SelectedNumberModel);
                    Number[idx] = NewNumber;
                    SelectedNumberModel = null;
                    NewNumber = new NumberModel();
                    Signal(nameof(NewNumber));
                }
            },
            () => SelectedNumberModel != null &&
                  NewNumber.Numberroom > 0 &&
                  !string.IsNullOrWhiteSpace(NewNumber.Type) &&
                  !string.IsNullOrWhiteSpace(NewNumber.Status) &&
                  NewNumber.Price > 0);

            RemoveNumber = new CommandMvvm(() =>
            {
                bool isUsed = BookingDB.GetDb().SelectAll().Any(b => b.RoomId == SelectedNumberModel.Id);
                if (isUsed)
                {
                    MessageBox.Show("Невозможно удалить номер: он используется в бронях.");
                    return;
                }
                if (NumberDB.GetDb().Remove(SelectedNumberModel))
                {
                    Number.Remove(SelectedNumberModel);
                    SelectedNumberModel = null;
                    NewNumber = new NumberModel();
                    Signal(nameof(NewNumber));
                }
            },
            () => SelectedNumberModel != null);
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}