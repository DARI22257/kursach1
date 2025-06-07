using kursach.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для EmployeesMemory.xaml
    /// </summary>
    public partial class EmployeesMemory : Window
    {
        public EmployeesMemory()
        {
            InitializeComponent();
            DataContext = new EmployeesMemoryMvvm();
        }
    }
}
