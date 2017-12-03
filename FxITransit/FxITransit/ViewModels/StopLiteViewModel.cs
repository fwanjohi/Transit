using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.ViewModels
{
    public class StopLiteViewModel
    {
        public List<StopLite> StopsFound { get; set; }
        public StopLite SelectedStop { get; set; }
        public StopLiteViewModel(List<StopLite> stopsFound)
        {
            StopsFound = stopsFound;
        }

        

    }
}
