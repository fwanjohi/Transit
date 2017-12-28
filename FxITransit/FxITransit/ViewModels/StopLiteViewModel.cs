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
        public List<Stop> StopsFound { get; set; }
        public Stop SelectedStop { get; set; }
        public string DisatanceDisplay { get; set; }
        public bool IsStart { get { return _isStart; } }

        private bool _isStart;
        public StopLiteViewModel(List<Stop> stopsFound, bool isStart)
        {
            StopsFound = stopsFound;
            _isStart = isStart;
        }

    }
}
