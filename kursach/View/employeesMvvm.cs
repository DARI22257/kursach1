using kursach.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace kursach.View
{
    public class employeesMvvm : BaseVM
    {
        private employees newemployees = new();
        public employees Newemployees
        {
            get => newemployees;
            set
            {
                newemployees = value;
                Signal();
            }
        }

        private employees selectedEmployees;
        public employees SelectedEmployees
        {
            get => selectedEmployees;
            set
            {
                selectedEmployees = value;
                if (value != null)
                {
                    // Копируем выбранного в форму редактирования
                    Newemployees = new employees
                    {
                        Id = value.Id,
                        name = value.name,
                        Jobtitle = value.Jobtitle,
                        Schedule = value.Schedule,
                        Phone = value.Phone
                    };
                }
                Signal();
            }
        }

        private ObservableCollection<employees> employees;
        public ObservableCollection<employees> Employees
        {
            get => employees;
            set
            {
                employees = value;
                Signal();
            }
        }

        public CommandMvvm InsertEmployees { get; set; }
        public CommandMvvm UpdateEmployees { get; set; }
        public CommandMvvm RemoveEmployees { get; set; }

        public employeesMvvm()
        {
            Employees = new ObservableCollection<employees>(employeesDB.GetDb().SelectAll());

            InsertEmployees = new CommandMvvm(() =>
            {
                employeesDB.GetDb().Insert(Newemployees);
                Employees.Add(Newemployees);
                Newemployees = new(); // очистить форму
                Signal(nameof(Newemployees));
                close?.Invoke();
            },
            () =>
                !string.IsNullOrEmpty(newemployees.name) &&
                !string.IsNullOrEmpty(newemployees.Jobtitle) &&
                !string.IsNullOrEmpty(newemployees.Phone));

            UpdateEmployees = new CommandMvvm(() =>
            {
                employeesDB.GetDb().Update(Newemployees);

                // обновить в списке
                var updated = Employees.IndexOf(SelectedEmployees);
                Employees[updated] = newemployees;
                SelectedEmployees = null;
                Newemployees = new();
                Signal(nameof(Newemployees));
            },
            () => SelectedEmployees != null &&
                  !string.IsNullOrEmpty(Newemployees.name) &&
                  !string.IsNullOrEmpty(Newemployees.Jobtitle) &&
                  !string.IsNullOrEmpty(Newemployees.Phone));

            RemoveEmployees = new CommandMvvm(() =>
            {
                employeesDB.GetDb().Remove(SelectedEmployees);
                Employees.Remove(SelectedEmployees);
                SelectedEmployees = null;
                Newemployees = new();
            },
            () => SelectedEmployees != null);
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}