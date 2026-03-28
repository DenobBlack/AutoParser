using AutoParser.Models;
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
using System.Windows.Shapes;

namespace AutoParser.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartEditWindow.xaml
    /// </summary>
    public partial class PartEditWindow : Window
    {
        public Part Part { get; set; }
        public PartEditWindow(Part part = null)
        {
            InitializeComponent();
            if (part != null)
            {
                Part = part;

                NameBox.Text = part.Name;
                NumberBox.Text = part.PartNumber;
                PriceBox.Text = part.Price.ToString();
            }
            else
            {
                Part = new Part();
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Part.Name = NameBox.Text;
            Part.PartNumber = NumberBox.Text;

            double.TryParse(PriceBox.Text, out double price);
            Part.Price = price;

            DialogResult = true;
        }
    }
}
