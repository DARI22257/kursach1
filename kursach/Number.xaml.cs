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
    /// Логика взаимодействия для Number.xaml
    /// </summary>
    public partial class Number : Window
    {
        public Number()
        {
            InitializeComponent();
            DataContext = new kursach.View.NumberMvvm();
        }
        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); mainWindow.Show();
        }
    }
}
