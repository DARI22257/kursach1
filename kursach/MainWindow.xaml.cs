﻿using System.Text;
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


        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Booking1 booking1 = new Booking1(); booking1.Show();
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
        private void NavigateButton_Click4(object sender, RoutedEventArgs e)
        {
            Guests guests = new Guests(); guests.Show();
        }
        private void NavigateButton_Click5(object sender, RoutedEventArgs e)
        {
            BookingMemory bookingMemory = new BookingMemory(); bookingMemory.Show();
        }
        private void NavigateButton_Click6(object sender, RoutedEventArgs e)
        {
            EmployeesMemory employeesMemory = new EmployeesMemory(); employeesMemory.Show();
        }
        private void NavigateButton_Click7(object sender, RoutedEventArgs e)
        {
            ServiceMemory serviceMemory = new ServiceMemory(); serviceMemory.Show();
        }
    }
}