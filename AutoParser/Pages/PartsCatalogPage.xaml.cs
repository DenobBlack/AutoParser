using AutoParser.Models;
using AutoParser.Service;
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

namespace AutoParser.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartsCatalogPage.xaml
    /// </summary>
    public partial class PartsCatalogPage : Page
    {
        List<Part> parts = new List<Part>();

        public PartsCatalogPage()
        {
            InitializeComponent();
            LoadParts();
        }
        private void LoadParts()
        {
            parts = PartService.GetAllParts();

            PartsGrid.ItemsSource = parts;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SearchBox.Text.ToLower();

            PartsGrid.ItemsSource = parts.FindAll(p =>
                p.Name.ToLower().Contains(text) ||
                p.PartNumber.ToLower().Contains(text));
        }

        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            var window = new PartEditWindow();

            if (window.ShowDialog() == true)
            {
                PartService.AddPart(window.Part);

                LoadParts();
            }
        }

        private void PartsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var part = PartsGrid.SelectedItem as Part;

            if (part == null)
                return;

            var window = new PartEditWindow(part);

            if (window.ShowDialog() == true)
            {
                PartService.UpdatePart(window.Part);

                LoadParts();
            }
        }

        private void PartsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var part = PartsGrid.SelectedItem as Part;

                if (part == null)
                    return;

                if (MessageBox.Show("Удалить деталь?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    PartService.DeletePart(part.Id);

                    LoadParts();
                }
            }
        }

        private void RemovePart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Part part = button.DataContext as Part;

            if (part == null)
                return;

            if (MessageBox.Show($"Удалить {part.Name} ?",
                "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PartService.DeletePart(part.Id);

                LoadParts();
            }
        }
    }
}
