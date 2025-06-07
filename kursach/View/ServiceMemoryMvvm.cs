using kursach.Model;
using kursachModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursach.View
{
    public class ServiceMemoryMvvm : INotifyPropertyChanged
    {
        public ObservableCollection<ServicesModel> ServicesModels { get; set; }
        private ObservableCollection<ServicesModel> allServices;

        private ServicesModel selectedServices;
        public ServicesModel SelectedServices
        {
            get => selectedServices;
            set
            {
                selectedServices = value;
                OnPropertyChanged();
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public CommandMvvm DeleteServices { get; set; }

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

        private string selectedTitle;
        public string SelectedTitle
        {
            get => selectedTitle;
            set
            {
                selectedTitle = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ObservableCollection<string> Title { get; set; }

        public ServiceMemoryMvvm()
        {
            LoadData();

            DeleteServices = new CommandMvvm(() =>
            {
                if (SelectedServices == null)
                    return;

                var emp = ServicesDB.GetDb().SelectAll()
                    .FirstOrDefault(e => e.Id == SelectedServices.Id);

                if (emp != null && ServicesDB.GetDb().Remove(emp))
                {
                    allServices.Remove(SelectedServices);
                    ServicesModels.Remove(SelectedServices);
                    SelectedServices = null;
                }
                else
                {
                    MessageBox.Show("Не удалось удалить сотрудника.");
                }
            },
            () => SelectedServices != null);
        }

        private void LoadData()
        {
            var raw = ServicesDB.GetDb().SelectAll();

            allServices = new ObservableCollection<ServicesModel>(
                raw.Select(emp => new ServicesModel
                {
                    Id = emp.Id,
                    Title = emp.Title,
                    Price = emp.Price
                }));

            ServicesModels = new ObservableCollection<ServicesModel>(allServices);

            Title = new ObservableCollection<string>(
                allServices.Select(e => e.Title).Distinct().OrderBy(p => p)
            );
        }

        private void ApplyFilter()
        {
            IEnumerable<ServicesModel> filtered = allServices;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string lower = SearchText.ToLower();

                filtered = filtered.Where(service =>
                    (service.Title?.ToLower().Contains(lower) ?? false) ||
                    service.Price.ToString().Contains(lower)
                );
            }

            if (!string.IsNullOrWhiteSpace(SelectedTitle))
            {
                filtered = filtered.Where(service => service.Title == SelectedTitle);
            }

            ServicesModels = new ObservableCollection<ServicesModel>(filtered);
            OnPropertyChanged(nameof(ServicesModels));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
