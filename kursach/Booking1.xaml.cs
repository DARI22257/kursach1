using kursach.View;
using System.Windows;

namespace kursach
{
    public partial class Booking1 : Window
    {
        public Booking1()
        {
            InitializeComponent();

            var vm = new BookingMvvm();
            DataContext = vm;
            vm.SetClose(this.Close); // передаём метод закрытия окна
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
