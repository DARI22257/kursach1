using kursach.Model;
using kursachModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.View
{
    internal class ServicesMvvm : BaseVM
    {
        private kursachModel.ServicesModel newServices = new();

        public kursachModel.ServicesModel NewServices
        {
            get => newServices;
            set
            {
                newServices = value;
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
        public ServicesMvvm()
        {
            Services = new ObservableCollection<ServicesModel>(ServicesDB.GetDb().SelectAll().Select(s => (ServicesModel)s));
            InsertServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Insert(newServices);
                Services.Add(NewServices);
                close?.Invoke();
            },
                () =>
                newServices.Price != 0 &&
                !string.IsNullOrEmpty(newServices.Title));
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
