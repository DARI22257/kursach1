using kursach.Model;
using kursachModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursach.View
{
    internal class ServicesMvvm : BaseVM
    {
        private ServicesModel newServices = new();
        public ServicesModel NewServices
        {
            get => newServices;
            set
            {
                newServices = value;
                Signal();
            }
        }

        private ServicesModel selectedService;
        public ServicesModel SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                Signal();
            }
        }

        private ObservableCollection<ServicesModel> services;
        public ObservableCollection<ServicesModel> Services
        {
            get => services;
            set
            {
                services = value;
                Signal();
            }
        }
        public CommandMvvm RemovesServices { get; set; }
        public CommandMvvm InsertServices { get; set; }


        public ServicesMvvm()
        {
            Services = new ObservableCollection<ServicesModel>(ServicesDB.GetDb().SelectAll());

            InsertServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Insert(NewServices);
                Services.Add(NewServices);
                NewServices = new(); // очищаем форму
                Signal(nameof(NewServices));
                close?.Invoke();
            },
            () =>
                NewServices.Price > 0 &&
                !string.IsNullOrWhiteSpace(NewServices.Title));

            RemovesServices = new CommandMvvm(() =>
            {

                var clien = MessageBox.Show("Вы уверены что хотите удалить клиента ?", "Подтверждение", MessageBoxButton.YesNo);

                if (clien == MessageBoxResult.Yes)
                {
                    employeesDB.GetDb().Remove(SelectedService);
                }
                SelectAll();

            }, () => true);
        }

        private void SelectAll()
        {
            Services = new ObservableCollection<Services>(ServicesDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
