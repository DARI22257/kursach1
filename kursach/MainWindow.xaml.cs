using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Guests guests = new Guests(); guests.Show();
        }
        private void NavigateButton_Click1(object sender, RoutedEventArgs e)
        {
            Employees employees = new Employees(); employees.Show();
        }
        private void NavigateButton_Click2(object sender, RoutedEventArgs e)
        {
            Services services = new Services(); services.Show();
        }
        private void NavigateButton_Click3(object sender, RoutedEventArgs e)
        {
            Number number = new Number(); number.Show();
        }
    }
}