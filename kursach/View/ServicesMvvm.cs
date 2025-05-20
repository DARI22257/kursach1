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
    internal class ServicesMvvm : BaseVM
    {
        private kursachModel.Services newServices = new();

        public kursachModel.Services NewServices
        {
            get => newServices;
            set
            {
                newServices = value;
                Signal();
            }
        }
        private ObservableCollection<Services> services;

        public ObservableCollection<Services> Services
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
           // Services = new ObservableCollection<Services>(ServicesDB.GetDb().SelectAll());
            InsertServices = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Insert(newServices);
                //Services.Add(NewServices);
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
