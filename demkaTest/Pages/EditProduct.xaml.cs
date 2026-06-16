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
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Page
    {
        public static Product product_;
        string filepath_;
        string filepath2;

        public EditProduct(Product product)
        {
            InitializeComponent();
            Initialize();
            product_ = product;
            InitializeProduct();
            
        }

        private void Initialize()
        {
            var context = Helper.GetContext();
            ProductSupplier.ItemsSource = context.Supplier.ToList();
            ProductManufacturer.ItemsSource = context.Manufacturer.ToList();
            ProductCategory.ItemsSource = context.Categories.ToList();
        }

        private void InitializeProduct()
        {
            ProductArticle.Text = product_.article.ToString();
            ProductName.Text = product_.name;
            ProductCost.Text = product_.cost.ToString();
            ProductDescription.Text = product_.description;
            ProductDiscount.Text = product_.discountPercent.ToString();
            ProductRemains.Text = product_.remains.ToString();
            ProductImage.Source = new BitmapImage(new Uri(product_.photo, UriKind.RelativeOrAbsolute));
            ProductSupplier.SelectedIndex = product_.supplierID - 1;
            ProductManufacturer.SelectedIndex = product_.manufacturerID - 1;
            ProductCategory.SelectedIndex = product_.categoryID - 1;
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
                    filepath_ = selectedFilePath;
                    string fileName = System.IO.Path.GetFileName(selectedFilePath);

                    string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagesFolder = System.IO.Path.Combine(projectPath, "Images");


                    string destinationPath = System.IO.Path.Combine(imagesFolder, fileName);
                    filepath2 = destinationPath;
                    MessageBox.Show(selectedFilePath);
                    MessageBox.Show(destinationPath);
                    File.Copy(selectedFilePath, destinationPath, true);




                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    ProductImage.Source = bitmap;
                    product_.photo = "\\" + "Images" + "\\" + fileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var context = Helper.GetContext();
                product_.manufacturerID = ProductManufacturer.SelectedIndex + 1;
                product_.supplierID = ProductSupplier.SelectedIndex + 1;
                product_.categoryID = ProductCategory.SelectedIndex + 1;
                product_.name = ProductName.Text;
                product_.article = ProductArticle.Text;
                if (Convert.ToDecimal(ProductCost.Text) < 0)
                {
                    MessageBox.Show("Цена не может быть меньше нуля");
                    return;
                }
                else
                {
                    product_.cost = Convert.ToDecimal(ProductCost.Text);
                }
                if (Convert.ToInt32(ProductDiscount.Text) < 0)
                {
                    MessageBox.Show("Скидка не может быть меньше нуля");
                    return;
                }
                else
                {
                    product_.cost = Convert.ToInt32(ProductDiscount.Text);
                }
                product_.description = ProductDescription.Text;
                product_.remains = Convert.ToInt32(ProductRemains.Text);

                var _product = context.Product.Where(p => p.id == product_.id).FirstOrDefault();
                _product = product_;
                context.SaveChanges();
                MessageBox.Show("Товар успешно отредактирован");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
