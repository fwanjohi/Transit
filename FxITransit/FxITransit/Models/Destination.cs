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
            PossibleRoutes = new List<SourceToDest>();
        }
       public List<SourceToDest> PossibleRoutes { get; set; }
    }
    public class SourceToDest
    {
        public SourceToDest(Stop sourceAddres)
        {
            StopsToNear = new List<Stop>();
            StartFrom = sourceAddres; 
        }
        
        public Stop StartFrom { get; set; }
        public Stop StopAt { get; set; }

        public List<Stop> StopsToNear { get; set; }
        
    }
}
