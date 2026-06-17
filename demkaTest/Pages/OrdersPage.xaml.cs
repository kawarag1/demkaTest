using demkaTest.Models;
using demkaTest.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demkaTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        List<Orders> orders_;
        public OrdersPage()
        {
            InitializeComponent();
            InitializeOrders();
        }
        
        private void InitializeOrders()
        {
            if (UserStatic.user.Roles.name == "Администратор")
            {
                AdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                AdminPanel.Visibility = Visibility.Hidden;
            }
            var context = Helper.GetContext();
            var orders = context.Orders.ToList();
            orders_ = orders;
            LViewOrders.ItemsSource = orders_;
        }

        private void LViewOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var order = LViewOrders.SelectedItem as Orders;
            NavigationService.Navigate(new EditOrder(order));
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrder());
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var context = Helper.GetContext();
            var order = LViewOrders.SelectedItem as Orders;
            var contents = context.OrderContents.Where(x => x.orderID == order.id).ToList();
            context.OrderContents.RemoveRange(contents);
            context.Orders.Remove(order);
            orders_.Remove(order);
            LViewOrders.ItemsSource = orders_;
            context.SaveChanges();
        }
    }
}
