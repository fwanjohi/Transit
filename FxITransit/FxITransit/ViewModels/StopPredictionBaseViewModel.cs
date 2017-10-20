using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;

namespace FxITransit.ViewModels
{
    public class StopPredictionBaseViewModel : BaseViewModel
    {
        public Action<List<Prediction>> OnPredictionsChanged { get;  set; }
    }

}
