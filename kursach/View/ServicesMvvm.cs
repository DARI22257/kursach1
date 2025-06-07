using kursach.Model;
using kursachModel;
using System;
using System.Collections.ObjectModel;

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
                if (value != null)
                {
                    NewServices = new ServicesModel
                    {
                        Id = value.Id,
                        Title = value.Title,
                        Price = value.Price
                    };
                }
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

        public CommandMvvm InsertServices { get; set; }
        public CommandMvvm UpdateServices { get; set; }
        public CommandMvvm RemovesServices { get; set; }

        public ServicesMvvm()
        {
            Services = new ObservableCollection<ServicesModel>(ServicesDB.GetDb().SelectAll());

            InsertServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Insert(NewServices);
                Services.Add(NewServices);
                NewServices = new();
                Signal(nameof(NewServices));
                close?.Invoke();
            },
            () =>
                NewServices.Price > 0 &&
                !string.IsNullOrWhiteSpace(NewServices.Title));

            UpdateServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Update(NewServices);
                var index = Services.IndexOf(SelectedService);
                Services[index] = NewServices;
                SelectedService = null;
                NewServices = new ServicesModel();
                Signal(nameof(NewServices));
            },
            () => SelectedService != null &&
                  !string.IsNullOrWhiteSpace(NewServices.Title));

            RemovesServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Remove(SelectedService);
                Services.Remove(SelectedService);
                SelectedService = null;
                NewServices = new();
                Signal(nameof(NewServices));
            },
            () => SelectedService != null);
        }

        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}