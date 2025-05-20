using kursach.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
