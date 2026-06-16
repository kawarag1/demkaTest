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
            var context = Helper.GetContext();
            var orders = context.Orders.ToList();
            orders_ = orders;
            LViewOrders.ItemsSource = orders_;
        }
    }
}
