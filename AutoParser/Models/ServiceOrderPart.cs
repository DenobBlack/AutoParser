using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Models
{
    public class ServiceOrderPart
    {
        public int Id { get; set; }

        public int ServiceOrderId { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
