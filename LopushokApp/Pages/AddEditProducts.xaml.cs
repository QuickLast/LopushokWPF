using LopushokApp.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LopushokApp.Pages
{
    public partial class AddEditProducts : Page
    {
        private Product product;
        private bool isEditMode;
        private List<Material> allMaterials;

        public AddEditProducts(Product _product)
        {
            InitializeComponent();

            ProdTypeCBox.ItemsSource = App.db.TypeProduct.ToList();

            product = _product ?? new Product();
            isEditMode = _product != null;

            if (product.ProductImage != null)
                ImageProduct.Source = product.ProductImage;

            allMaterials = App.db.Material.ToList();

            if (product.ProductMaterial?.Any() == true)
            {
                foreach (var item in product.ProductMaterial)
                    allMaterials.RemoveAll(m => m.ID == item.Material.ID);
            }

            MaterialsComboBox.ItemsSource = allMaterials;
            UpdateMaterialsDataGrid();

            DataContext = product;
        }

        private void DownloadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    product.Image = imageBytes;

                    var bitmap = new BitmapImage();
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                    }

                    ImageProduct.Source = bitmap;
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка загрузки изображения: {ex.Message}");
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (product.ProductSale.Any())
            {
                ShowError("Удаление невозможно, так как есть продажи");
                return;
            }

            App.db.Product.Remove(product);
            App.db.ProductMaterial.RemoveRange(App.db.ProductMaterial.Where(x => x.ID == product.ID));
            App.db.SaveChanges();

            ShowMessage("Успешно удалено");
            App.main.BodyFrame.NavigationService.Navigate(new ProductsList());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            App.main.BodyFrame.NavigationService.Navigate(new ProductsList());
        }

        private void UpdateMaterialsDataGrid()
        {
            MaterialsDataGrid.ItemsSource = product.ProductMaterial?.ToList() ?? new List<ProductMaterial>();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsComboBox.SelectedItem is Material selectedMaterial)
            {
                if (int.TryParse(MaterialCountTextBox.Text, out var count) && count > 0)
                {
                    if (product.ProductMaterial == null)
                        product.ProductMaterial = new HashSet<ProductMaterial>();

                    var newMaterial = new ProductMaterial
                    {
                        Material = selectedMaterial,
                        Count_Mat_Fot_One = count
                    };

                    product.ProductMaterial.Add(newMaterial);
                    App.db.ProductMaterial.Add(newMaterial);
                    App.db.SaveChanges();

                    ShowMessage("Материал добавлен!");
                    UpdateMaterialsDataGrid();

                    allMaterials.RemoveAll(m => m.ID == selectedMaterial.ID);
                    MaterialsComboBox.ItemsSource = allMaterials;

                    MaterialCountTextBox.Clear();
                    MaterialsComboBox.SelectedIndex = -1;
                }
                else
                {
                    ShowError("Введите корректное количество!");
                }
            }
            else
            {
                ShowError("Выберите материал!");
            }
        }

        private void RemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsDataGrid.SelectedItem is ProductMaterial selectedMaterial)
            {
                product.ProductMaterial.Remove(selectedMaterial);
                App.db.ProductMaterial.Remove(selectedMaterial);
                App.db.SaveChanges();

                ShowMessage("Материал удален!");

                allMaterials.Add(selectedMaterial.Material);
                MaterialsComboBox.ItemsSource = allMaterials;

                MaterialCountTextBox.Clear();
                MaterialsComboBox.SelectedIndex = -1;

                UpdateMaterialsDataGrid();
            }
            else
            {
                ShowError("Выберите материал для удаления!");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(product.Articul.ToString()) || string.IsNullOrEmpty(artTB.Text) ||
                string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(nameTB.Text) ||
                product.TypeProduct == null ||
                product.Departament == null || string.IsNullOrEmpty(depTB.Text) ||
                product.Min_Price_For_Agent == null || string.IsNullOrEmpty(mpTB.Text))
            {
                ShowError("Заполните все обязательные поля!");
                return;
            }

            if (!decimal.TryParse(product.Min_Price_For_Agent.ToString(), out var minCost) || minCost <= 0)
            {
                ShowError("Минимальная стоимость должна быть числом больше 0!");
                return;
            }

            if (product.Count_Employee < 0)
            {
                ShowError("Количество людей должно быть положительным числом!");
                return;
            }

            if (product.Departament < 0)
            {
                ShowError("Номер цеха должен быть положительным числом!");
                return;
            }

            if (App.db.Product.Any(x => x.Articul == product.Articul && x.ID != product.ID))
            {
                ShowError("Артикул занят.");
                return;
            }

            try
            {
                if (isEditMode)
                {
                    var existingProduct = App.db.Product.FirstOrDefault(p => p.ID == product.ID);
                    if (existingProduct != null)
                    {
                        existingProduct.Articul = product.Articul;
                        existingProduct.Name = product.Name;
                        existingProduct.Count_Employee = product.Count_Employee;
                        existingProduct.Departament = product.Departament;
                        existingProduct.Min_Price_For_Agent = product.Min_Price_For_Agent;
                        existingProduct.Description = product.Description;
                        existingProduct.Image = product.Image;
                    }
                }
                else
                {
                    App.db.Product.Add(product);
                }

                App.db.SaveChanges();
                ShowMessage("Продукт успешно сохранен!");
                App.main.BodyFrame.NavigationService.Navigate(new Pages.ProductsList());
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
