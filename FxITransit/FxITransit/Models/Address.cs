using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Platform.Services.Geolocation;

namespace FxITransit.Models
{
    public class GoogleAddress
    {
        public string FormattedAddress { get; set; }
        public string RequestedAddress { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public double Distance { get; set; }

        public string Display
        {
            get
            {
                return  $"{Name}, {Distance} miles away"; 
            }
        }
    }
}
