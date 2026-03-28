using AutoParser.Models;
using AutoParser.Pages;
using AutoParser.Service;
using System.Text;
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

namespace AutoParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool menuCollapsed = false;
        public MainWindow()
        {
            InitializeComponent();
            DatabaseService.Initialize();
            NavigateWithAnimation(new CreateTOPage());
            SetActiveButton(BtnCreateTO);
        }
        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();

            if (!menuCollapsed)
            {
                animation.To = 60;

                LogoBlock.Visibility = Visibility.Collapsed;
                CreateToText.Visibility = Visibility.Collapsed;
                CarsText.Visibility = Visibility.Collapsed;
                PartsText.Visibility = Visibility.Collapsed;
                OrdersText.Visibility = Visibility.Collapsed;

                menuCollapsed = true;
            }
            else
            {
                animation.To = 200;

                LogoBlock.Visibility = Visibility.Visible;
                CreateToText.Visibility = Visibility.Visible;
                CarsText.Visibility = Visibility.Visible;
                PartsText.Visibility = Visibility.Visible;
                OrdersText.Visibility = Visibility.Visible;

                menuCollapsed = false;
            }

            animation.Duration = TimeSpan.FromMilliseconds(200);

            MenuPanel.BeginAnimation(FrameworkElement.WidthProperty, animation);
        }
        private void SetActiveButton(Button active)
        {
            BtnCreateTO.Background = Brushes.Transparent;
            BtnCars.Background = Brushes.Transparent;
            BtnParts.Background = Brushes.Transparent;
            BtnOrders.Background = Brushes.Transparent;

            active.Background = new SolidColorBrush(Color.FromRgb(55, 65, 81));
        }
        private void OpenCreateTO_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithAnimation(new CreateTOPage());
            SetActiveButton(BtnCreateTO);
        }
        private void OpenOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithAnimation(new OrdersPage());
            SetActiveButton(BtnOrders);
        }
        private void OpenCars_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithAnimation(new CarsPage());
            SetActiveButton(BtnCars);
        }
        private void OpenParts_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithAnimation(new PartsCatalogPage());
            SetActiveButton(BtnParts);
        }
        private void NavigateWithAnimation(Page page)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));

            fadeOut.Completed += (s, e) =>
            {
                MainFrame.Navigate(page);

                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
                MainFrame.BeginAnimation(Frame.OpacityProperty, fadeIn);
            };

            MainFrame.BeginAnimation(Frame.OpacityProperty, fadeOut);
        }
    }
}