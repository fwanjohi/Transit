using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class PredictionsViewModel : StopPredictionBaseViewModel
    {

        public Command GoogleDirectionsCommand { get; set; }
        public Command ChangeFavoriteCommand { get; set; }

        public Stop Stop { get; private set; }

        public PredictionsViewModel(Stop stop)
        {
            OnPredictionsChanged = UpdatePredictions;
            
            Stop = stop;
            Title = $"Predictions - {stop.TitleDisplay }";
            ChangeFavoriteCommand = new Command(async () =>  StopOptionsHelper.Instance.AddFavorite(Stop));
        }
        
        
    }
}
