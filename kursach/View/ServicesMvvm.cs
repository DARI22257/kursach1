using kursach.Model;
using System;
using System.Collections.Generic;
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

        public CommandMvvm InsertClient { get; set; }
        public ServicesMvvm()
        {
            InsertClient = new CommandMvvm(() =>
            {
                ServicesDB.GetDb().Insert(NewServices);
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
