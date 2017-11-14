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
        private ObservableRangeCollection<Stop> _faves; 
        public ObservableRangeCollection<Stop> FavoriteStops
        {
            get => _faves;
            set
            {
                _faves = value;
                OnPropertyChanged("FavoriteStops");
            }
        }

        public FavoritesViewModel()
        {
            FavoriteStops = new ObservableRangeCollection<Stop>();
            OnPredictionsChanged = UpdatePredictions;
        }



    }
}
