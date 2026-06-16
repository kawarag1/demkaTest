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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        List<Product> products_;
        public MainPage()
        {
            InitializeComponent();
            InitializeProducts();
            InitializeRole();
            InitializeManufacturers();
        }

        private void InitializeProducts()
        {
            var context = Helper.GetContext();
            var products = context.Product.ToList();
            products_ = products;
            LViewMaterials.ItemsSource = products_;
        }

        private void InitializeRole()
        {
            if (UserStatic.user == null || UserStatic.user.Roles.name == "Авторизованный пользователь")
            {
                FilterPanel.Visibility = Visibility.Hidden;
                AdminPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                if (UserStatic.user.Roles.name == "Администратор")
                {
                    AdminPanel.Visibility = Visibility.Visible;
                    Orders.Visibility = Visibility.Visible;
                    DeleteProduct.Visibility = Visibility.Visible;
                    AddProduct.Visibility = Visibility.Visible;
                }
                else if (UserStatic.user.Roles.name == "Менеджер")
                {
                    AdminPanel.Visibility = Visibility.Visible;
                    Orders.Visibility = Visibility.Visible;
                    DeleteProduct.Visibility = Visibility.Hidden;
                    AddProduct.Visibility = Visibility.Hidden;
                }
            }
        }

        private void InitializeManufacturers()
        {
            var context = Helper.GetContext();
            var manufacturers = context.Manufacturer.ToList();
            manufacturers.Insert(0, new Manufacturer { id = 0, name = "Все производители" });
            ManufacturerSort.ItemsSource = manufacturers;
            ManufacturerSort.SelectedIndex = 0;
        }

        private void DiscountSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiscountSort.SelectedIndex == 0)
            {
                LViewMaterials.ItemsSource = products_.OrderBy(x => x.discountPercent).ToList();
            }
            else
            {
                LViewMaterials.ItemsSource = products_.OrderByDescending(x => x.discountPercent).ToList();
            }
        }

        private void CostSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CostSort.SelectedIndex == 0)
            {
                LViewMaterials.ItemsSource = products_.OrderBy(x => x.cost).ToList();
            }
            else
            {
                LViewMaterials.ItemsSource = products_.OrderByDescending(x => x.cost).ToList();
            }
        }

        private void CountSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountSort.SelectedIndex == 0)
            {
                LViewMaterials.ItemsSource = products_.OrderBy(x => x.remains).ToList();
            }
            else
            {
                LViewMaterials.ItemsSource = products_.OrderByDescending(x => x.remains).ToList();
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProduct());
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var context = Helper.GetContext();
                var product = LViewMaterials.SelectedItem as Product;
                products_.Remove(product);
                LViewMaterials.ItemsSource = products_;
                context.Product.Remove(product);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данный товар находится в заказе, удаление невозможно");
            }
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        private void ManufacturerSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = Helper.GetContext();
            var allProducts = context.Product.ToList();

            if (ManufacturerSort.SelectedIndex == 0)
            {
                LViewMaterials.ItemsSource = allProducts;
            }
            else
            {
                var selectedManufacturer = ManufacturerSort.SelectedItem as Manufacturer;
                if (selectedManufacturer != null)
                {
                    LViewMaterials.ItemsSource = products_.Where(x => x.manufacturerID == selectedManufacturer.id).ToList();
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text;
            LViewMaterials.ItemsSource = products_.Where(x => x.name.Contains(searchText));
        }

        private void LViewMaterials_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserStatic.user == null)
            {
                return;
            }
            else if (UserStatic.user.Roles.name == "Авторизованный клиент")
            {
                return;
            }
            else
            {
                var product = LViewMaterials.SelectedItem as Product;
                NavigationService.Navigate(new EditProduct(product));
            }
        }
    }
}
