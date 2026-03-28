using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParser.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string LicensePlate { get; set; }
        public string DisplayName
        {
            get { return $"{Brand} {Model} ({VIN})"; }
        }
    }
}
