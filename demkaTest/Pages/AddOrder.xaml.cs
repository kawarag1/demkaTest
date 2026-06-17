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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        public AddOrder()
        {
            InitializeComponent();
            InitializeAdresses();
            InitializeStatuses();
        }

        private void InitializeAdresses()
        {
            var context = Helper.GetContext();
            AddressBox.ItemsSource = context.PickUpPoints.ToList();
        }

        private void InitializeStatuses()
        {
            var context = Helper.GetContext();
            Statuses.ItemsSource = context.Statuses.ToList();
        }
        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var context = Helper.GetContext();
            Orders order = new Orders();
            order.CreateDate = CreateDate.SelectedDate.Value;
            order.DeliveryDate = DeliveryDate.SelectedDate.Value;
            order.Code = 111;
            order.UserID = UserStatic.user.id;
            order.PickUpPointID = AddressBox.SelectedIndex + 1;
            order.StatusID = Statuses.SelectedIndex + 1;
            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}
