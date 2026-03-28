using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Models
{
    public class ServiceOrder
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public int ServiceTypeId { get; set; }

        public DateTime Date { get; set; }

        public double Total { get; set; }
    }
}
