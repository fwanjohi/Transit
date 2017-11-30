using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    public class StopLite
    {
        public string Title { get; set; }
        public string Tag { get; set; }
        public string DistanceDisplay { get; set; }
        public string AgencyTitle { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Distance { get; set; }

    }
}
