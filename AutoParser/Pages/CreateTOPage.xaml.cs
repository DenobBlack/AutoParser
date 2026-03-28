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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoParser.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateTOPage.xaml
    /// </summary>
    public partial class CreateTOPage : Page
    {
        private bool historyVisible = false;
        private List<Part> currentParts = new List<Part>();
        List<Car> allCars = new List<Car>();
        public CreateTOPage()
        {
            InitializeComponent();
            LoadCars();
            LoadServiceTypes();
        }
        private void LoadCars()
        {
            allCars = CarService.GetCars();

            CarComboBox.ItemsSource = allCars;
        }
        private void LoadServiceTypes()
        {
            var types = ServiceTypeService.GetTypes();
            ServiceTypeComboBox.ItemsSource = types;
        }
        

        private void CarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Car car = CarComboBox.SelectedItem as Car;

            if (car == null)
                return;

            LoadHistory(car.Id);
        }
        private void LoadHistory(int carId)
        {
            var history = CarService.GetHistory(carId);

            HistoryGrid.ItemsSource = history;
        }
        private void ToggleHistory(object sender, RoutedEventArgs e)
        {
            if (!historyVisible)
            {
                HistoryRow.Height = new GridLength(150);
                HistoryGrid.Visibility = Visibility.Visible;
                historyVisible = true;
            }
            else
            {
                HistoryRow.Height = new GridLength(0);
                HistoryGrid.Visibility = Visibility.Collapsed;
                historyVisible = false;
            }
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var car = CarComboBox.SelectedItem as Car;
            var type = ServiceTypeComboBox.SelectedItem as ServiceType;

            if (car == null || type == null)
            {
                MessageBox.Show("Выберите автомобиль и тип ТО");
                return;
            }

            double total = currentParts.Sum(p => p.Sum);

            ServiceOrderService.CreateOrder(car.Id, type.Id, total);

            MessageBox.Show("ТО создано", "");

            LoadHistory(car.Id);
        }

        private void ServiceTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = ServiceTypeComboBox.SelectedItem as ServiceType;

            if (type == null)
                return;

            currentParts = PartService.GetPartsForService(type.Id);

            PartsGrid.ItemsSource = currentParts;

            UpdateTotal();
        }
        private void UpdateTotal()
        {
            double total = currentParts.Sum(p => p.Sum);

            TotalText.Text = total.ToString() + " ₽";
        }

        private void PartSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = PartSearchBox.Text;

            if (text.Length < 2)
            {
                PartsSearchList.Visibility = Visibility.Collapsed;
                return;
            }

            var results = PartService.SearchParts(text);

            PartsSearchList.ItemsSource = results;

            if (results.Count > 0)
                PartsSearchList.Visibility = Visibility.Visible;
            else
                PartsSearchList.Visibility = Visibility.Collapsed;
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
                currentParts.Remove(part);

                PartsGrid.ItemsSource = null;
                PartsGrid.ItemsSource = currentParts;

                UpdateTotal();
            }
        }
        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            var part = PartsSearchList.SelectedItem as Part;

            if (part == null)
                return;

            var existing = currentParts.FirstOrDefault(p => p.Id == part.Id);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                currentParts.Add(new Part
                {
                    Id = part.Id,
                    Name = part.Name,
                    Price = part.Price,
                    Quantity = 1
                });
            }

            PartsGrid.ItemsSource = null;
            PartsGrid.ItemsSource = currentParts;

            UpdateTotal();

            PartsSearchList.Visibility = Visibility.Collapsed;
            PartSearchBox.Text = "";
        }

        private void PartsSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var part = PartsSearchList.SelectedItem as Part;

            if (part == null)
                return;

            var existing = currentParts.FirstOrDefault(p => p.Id == part.Id);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                currentParts.Add(new Part
                {
                    Id = part.Id,
                    Name = part.Name,
                    Price = part.Price,
                    Quantity = 1
                });
            }

            PartsGrid.ItemsSource = null;
            PartsGrid.ItemsSource = currentParts;

            UpdateTotal();

            PartsSearchList.Visibility = Visibility.Collapsed;
            PartSearchBox.Text = "";
        }

        private void PartsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(UpdateTotal));
        }

        private void PartsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var part = PartsGrid.SelectedItem as Part;

                if (part == null)
                    return;

                currentParts.Remove(part);

                PartsGrid.ItemsSource = null;
                PartsGrid.ItemsSource = currentParts;

                UpdateTotal();
            }
        }

        private void CarComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            string text = CarComboBox.Text.ToLower();

            var filtered = allCars.Where(c =>
                c.DisplayName.ToLower().Contains(text) ||
                c.VIN.ToLower().Contains(text))
                .ToList();

            CarComboBox.ItemsSource = filtered;

            CarComboBox.IsDropDownOpen = true;
        }
    }
}
