using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class EmployeeMem
    {
        public int Id { get; set; } // нужен для удаления

        public string name { get; set; }
        public string Jobtitle { get; set; }
        public DateTime Schedule { get; set; } = DateTime.Now;
        public string Phone { get; set; }
    }
}
