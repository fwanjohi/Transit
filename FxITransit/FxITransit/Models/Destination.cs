using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    public class Destination
    {
        public Destination()
        {
            PossibleRoutes = new List<PossibleRoute>();
            DestinationTitle = "Your Destination";
        }
        public List<PossibleRoute> PossibleRoutes { get; set; }
        public string DestinationTitle { get; set; }
    }
    public class PossibleRoute
    {
        
        public PossibleRoute(Stop sourceAddres, string start = "Current location", string end = "Your destination")
        {
            StopsToNear = new List<Stop>();
            StartFrom = sourceAddres;
            DestinationTitle = end;
            SourceTitle = start;
        }

        public Stop StartFrom { get; set; }
        public Stop StopAt { get; set; }

        public List<Stop> StopsToNear { get; set; }

        public string DestinationTitle { get; set; }
        public string SourceTitle { get; set; }

        public string DestinationDistance
        {
            get => StopAt.WalkingDistanceTime;
        }

    }
}
