using kursach.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public CommandMvvm RemovesEmployees { get; set; }
        public CommandMvvm InsertEmployees { get; set; }
        public employeesMvvm()
        {
            Employees = new ObservableCollection<employees>(employeesDB.GetDb().SelectAll());
            InsertEmployees = new CommandMvvm(() =>
            {
                employeesDB.GetDb().Insert(newemployees);
                Employees.Add(Newemployees);
                close?.Invoke();
            },
                () =>
                !string.IsNullOrEmpty(newemployees.name) &&
                !string.IsNullOrEmpty(newemployees.Jobtitle) &&
                !string.IsNullOrEmpty(newemployees.Phone));

            RemovesEmployees = new CommandMvvm(() =>
            {

                var clien = MessageBox.Show("Вы уверены что хотите удалить клиента ?", "Подтверждение", MessageBoxButton.YesNo);

                if (clien == MessageBoxResult.Yes)
                {
                    employeesDB.GetDb().Remove(SelectedEmployees);
                }
                SelectAll();

            }, () => true);
        }

        private void SelectAll()
        {
            Employees = new ObservableCollection<employees>(employeesDB.GetDb().SelectAll());
        }



            Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
