using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;

namespace FxITransit.ViewModels
{
    public class FavoritesViewModel : StopPredictionBaseViewModel
    {
        public ObservableRangeCollection<Stop> FavoriteStops { get => StopOptionsHelper.Instance.MySettings.FavoriteStops; }
        public FavoritesViewModel()
        {
            OnPredictionsChanged = UpdatePredictions;
        }



    }
}
