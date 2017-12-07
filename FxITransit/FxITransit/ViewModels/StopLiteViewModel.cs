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
        public string DisatanceDisplay { get; set; }
        public bool IsStart { get { return _isStart; } }

        private bool _isStart;
        public StopLiteViewModel(List<StopLite> stopsFound, bool isStart = false)
        {
            StopsFound = stopsFound;
            _isStart = isStart;

        }

        

    }
}
