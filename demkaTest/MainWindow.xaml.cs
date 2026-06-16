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
using demkaTest.Pages;
using demkaTest.Services;

namespace demkaTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new AuthPage());
        }

        private void FrmMain_Navigated(object sender, NavigationEventArgs e)
        {
            if (FrmMain.Content is MainPage mainPage)
            {
                Header.Text = "Товары";
                BackBtn.Visibility = Visibility.Visible;
                if (UserStatic.user != null)
                {   
                    FIO.Text = UserStatic.user.surname + " " + UserStatic.user.name + " " + UserStatic.user.patronymic;
                }
                else
                {
                    FIO.Text = "Гость";
                }
                
            }
            else if (FrmMain.Content is AuthPage)
            {
                BackBtn.Visibility = Visibility.Hidden;
                FIO.Text = "";
                UserStatic.user = null;
            }
            else if (FrmMain.Content is AddProduct)
            {
                Header.Text = "Добавление товара";
            }
            else if (FrmMain.Content is EditProduct)
            {
                Header.Text = "Редактирование товара";
            }
            else if (FrmMain.Content is OrdersPage)
            {
                Header.Text = "Заказы";
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                FrmMain.GoBack();
            }
        }
    }
}
