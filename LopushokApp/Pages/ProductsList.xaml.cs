using LopushokApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LopushokApp.Pages
{
    public partial class ProductsList : Page
    {
        private List<Product> allProd;
        private List<Product> filtProd;
        private readonly List<TypeProduct> allTypeProd;
        private int currPage = 1;
        private const int ProdsPerPage = 20;

        public ProductsList()
        {
            InitializeComponent();
            allProd = App.db.Product.ToList();
            filtProd = new List<Product>(allProd);
            allTypeProd = App.db.TypeProduct.ToList();
            LoadProducts();
        }

        private void LoadProducts()
        {
            int maxPages = Math.Max(1, (int)Math.Ceiling((double)filtProd.Count / ProdsPerPage));
            ProdItemsIC.ItemsSource = filtProd.Skip((currPage - 1) * ProdsPerPage).Take(ProdsPerPage).ToList();
            PageInfoTxtBlock.Text = string.Format("{0} / {1}", currPage, maxPages);
        }

        private void SearchTxtBox_TextChanged(object sender, TextChangedEventArgs e) { GetItems(); }
        private void SortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { GetItems(); }
        private void FilterCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { GetItems(); }

        private void GetItems()
        {
            string searchText = SearchTxtBox.Text.ToLower();
            filtProd = string.IsNullOrWhiteSpace(searchText)
                ? new List<Product>(allProd)
                : allProd.Where(p => p.Name.ToLower().Contains(searchText) || (p.Description ?? "").ToLower().Contains(searchText)).ToList();

            filtProd = ApplyFilters(filtProd);
            filtProd = ApplySorting(filtProd);
            currPage = 1;
            LoadProducts();
        }

        private List<Product> ApplySorting(List<Product> filtProd)
        {
            switch (SortCmbBox.SelectedIndex)
            {
                case 0:
                    filtProd = filtProd.OrderBy(p => p.Name).ToList();
                    break;
                case 1:
                    filtProd = filtProd.OrderByDescending(p => p.Name).ToList();
                    break;
                case 2:
                    filtProd = filtProd.OrderBy(p => p.Departament).ToList();
                    break;
                case 3:
                    filtProd = filtProd.OrderByDescending(p => p.Departament).ToList();
                    break;
                case 4:
                    filtProd = filtProd.OrderBy(p => p.Min_Price_For_Agent).ToList();
                    break;
                case 5:
                    filtProd = filtProd.OrderByDescending(p => p.Min_Price_For_Agent).ToList();
                    break;
            }
            return filtProd;
        }

        private List<Product> ApplyFilters(List<Product> filtProd)
        {
            if (FilterCmbBox.SelectedIndex > 0)
            {
                int selectedTypeId = FilterCmbBox.SelectedIndex;
                filtProd = filtProd.Where(p => p.ID_Type == selectedTypeId).ToList();
            }
            else if (FilterCmbBox.SelectedIndex == 0)
            {

            }
            return filtProd;
        }

        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currPage > 1)
            {
                currPage--;
                LoadProducts();
            }
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currPage < Math.Ceiling((double)filtProd.Count / ProdsPerPage))
            {
                currPage++;
                LoadProducts();
            }
        }

        private void ProdItemsIC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject clickedElement = e.OriginalSource as DependencyObject;
            while (clickedElement != null && !(clickedElement is ContentPresenter))
            {
                try
                {
                    clickedElement = VisualTreeHelper.GetParent(clickedElement);
                }
                catch
                {
                    break;
                }
                if (clickedElement == null)
                    break;
            }

            if (clickedElement is ContentPresenter presenter && presenter.DataContext is Product)
            {
                Product selectedProd = (Product)presenter.DataContext;
                App.main.BodyFrame.NavigationService.Navigate(new Pages.AddEditProducts(selectedProd));
            }
        }

        private void AddProdBtn_Click(object sender, RoutedEventArgs e)
        {
            App.main.BodyFrame.NavigationService.Navigate(new Pages.AddEditProducts(null));
        }
    }
}
