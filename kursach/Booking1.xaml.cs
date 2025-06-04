using kursach.Model;
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
    /// Логика взаимодействия для Booking1.xaml
    /// </summary>
    public partial class Booking1 : Window
    {
        public Booking1()
        {
            InitializeComponent();
            DataContext = new BookingMvvm();
        }
        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); mainWindow.Show();
        }

    }
}
