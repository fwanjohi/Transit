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
        private List<Stop> _faves; 
        public List<Stop> FavoriteStops
        {
            get => _faves;
            set
            {
                _faves = value;
                OnPropertyChanged("FavoriteStops");
            }
        }

        public FavoritesViewModel( List<Stop> faves)
        {
            _faves = faves;
            OnPredictionsChanged = UpdatePredictions;
        }



    }
}
