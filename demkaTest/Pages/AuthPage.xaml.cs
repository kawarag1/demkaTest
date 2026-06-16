using demkaTest.Models;
using demkaTest.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = loginBox.Text;
                string pwd = PwdBox.Password.ToString();
                var context = Helper.GetContext();
                var user = context.Users.Where(x => x.login == login && x.password == pwd).Include("Roles").FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден");
                }
                else
                {
                    UserStatic.user = user;
                    NavigationService.Navigate(new MainPage());
                    MessageBox.Show($"Добро пожаловать, {user.name} {user.surname}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            UserStatic.user = null;
            NavigationService.Navigate(new MainPage());
            MessageBox.Show($"Вы вошли как гость");
        }
    }
}
