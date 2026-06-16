using demkaTest.Models;
using demkaTest.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        string filepath_;
        string filepath2;
        string path;
        public AddProduct()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            var context = Helper.GetContext();
            ProductSupplier.ItemsSource = context.Supplier.ToList();
            ProductManufacturer.ItemsSource = context.Manufacturer.ToList();
            ProductCategory.ItemsSource = context.Categories.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            var context = Helper.GetContext();
            product.manufacturerID = ProductManufacturer.SelectedIndex + 1;
            product.supplierID = ProductSupplier.SelectedIndex + 1;
            product.categoryID = ProductCategory.SelectedIndex + 1;
            product.name = ProductName.Text;
            product.article = ProductArticle.Text;
            if (Convert.ToDecimal(ProductCost.Text) < 0)
            {
                MessageBox.Show("Цена не может быть меньше нуля");
                return;
            }
            else
            {
                product.cost = Convert.ToDecimal(ProductCost.Text);
            }
            if (Convert.ToInt32(ProductDiscount.Text) < 0)
            {
                MessageBox.Show("Скидка не может быть меньше нуля");
                return;
            }
            else
            {
                product.cost = Convert.ToInt32(ProductDiscount.Text);
            }
            product.description = ProductDescription.Text;
            product.remains = Convert.ToInt32(ProductRemains.Text);
            if (path == null)
            {
                product.photo = "\\Images\\default.png";
            }
            else
            {
                product.photo = path;
            }
            product.unitID = 1;
            context.Product.Add(product);
            context.SaveChanges();
            MessageBox.Show("Товар успешно создан");
        }

        private void ProductImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите изображение";
            dialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Все файлы (*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFilePath = dialog.FileName;
                    string fileName = System.IO.Path.GetFileName(selectedFilePath);

                    string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagesFolder = System.IO.Path.Combine(projectPath, "Images");


                    string destinationPath = System.IO.Path.Combine(imagesFolder, fileName);
                    File.Copy(selectedFilePath, destinationPath, true);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    ProductImage.Source = bitmap;
                    path = "\\" + "Images" + "\\" + fileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
