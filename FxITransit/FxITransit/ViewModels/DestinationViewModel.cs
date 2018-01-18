using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.ViewModels
{
    public class DestinationViewModel
    {

        private bool _isStart;
        public Destination Destination { get; set; }
        

        public DestinationViewModel(Destination destination)
        {
            Destination = destination;
            //StopsFound = stopsFound;
            //_isStart = isStart;
        }

    }
}
