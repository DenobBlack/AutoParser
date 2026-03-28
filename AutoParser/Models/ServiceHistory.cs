using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Models
{
    public class ServiceHistory
    {
        public string Date { get; set; }

        public string ServiceName { get; set; }

        public int Mileage { get; set; }

        public double Total { get; set; }
    }
}
