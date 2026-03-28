using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private int quantity;
        public int Quantity {
            get => quantity;
            set
            {
                quantity = value < 1 ? 1 : value;
            }
        }
        public double Price { get; set; }
        public string PartNumber { get; set; }
        public double Sum => Quantity * Price;
        public string DisplayName => $"{Name} ({PartNumber})";
    }
}
