using kursach.Model;
using kursachModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace kursach.View
{
    public class EmployeesMemoryMvvm : INotifyPropertyChanged
    {
        public ObservableCollection<EmployeeMem> Employees { get; set; }
        private ObservableCollection<EmployeeMem> allEmployees;

        private EmployeeMem selectedEmployee;
        public EmployeeMem SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public CommandMvvm DeleteEmployee { get; set; }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private string selectedPosition;
        public string SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ObservableCollection<string> Positions { get; set; }

        public EmployeesMemoryMvvm()
        {
            LoadData();

            DeleteEmployee = new CommandMvvm(() =>
            {
                if (SelectedEmployee == null)
                    return;

                var emp = employeesDB.GetDb().SelectAll()
                    .FirstOrDefault(e => e.Id == SelectedEmployee.Id);

                if (emp != null && employeesDB.GetDb().Remove(emp))
                {
                    allEmployees.Remove(SelectedEmployee);
                    Employees.Remove(SelectedEmployee);
                    SelectedEmployee = null;
                }
                else
                {
                    MessageBox.Show("Не удалось удалить сотрудника.");
                }
            },
            () => SelectedEmployee != null);
        }

        private void LoadData()
        {
            var raw = employeesDB.GetDb().SelectAll();

            allEmployees = new ObservableCollection<EmployeeMem>(
                raw.Select(emp => new EmployeeMem
                {
                    Id = emp.Id,
                    name = emp.name,
                    Jobtitle = emp.Jobtitle,
                    Schedule = emp.Schedule,
                    Phone = emp.Phone
                }));

            Employees = new ObservableCollection<EmployeeMem>(allEmployees);

            Positions = new ObservableCollection<string>(
                allEmployees.Select(e => e.Jobtitle).Distinct().OrderBy(p => p)
            );
        }

        private void ApplyFilter()
        {
            IEnumerable<EmployeeMem> filtered = allEmployees;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lower = SearchText.ToLower();

                filtered = filtered.Where(emp =>
                    (emp.name?.ToLower().Contains(lower) ?? false) ||
                    (emp.Phone?.ToLower().Contains(lower) ?? false) ||
                    (emp.Jobtitle?.ToLower().Contains(lower) ?? false) ||
                    emp.Schedule.ToString("dd.MM.yyyy").ToLower().Contains(lower)
                );
            }

            if (!string.IsNullOrWhiteSpace(SelectedPosition))
            {
                filtered = filtered.Where(emp => emp.Jobtitle == SelectedPosition);
            }

            Employees = new ObservableCollection<EmployeeMem>(filtered);
            OnPropertyChanged(nameof(Employees));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
