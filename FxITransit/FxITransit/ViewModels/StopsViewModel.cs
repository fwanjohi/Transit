using FxITransit.Helpers;
using FxITransit.Models;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class StopsViewModel : BaseViewModel
    {
        public Direction Direction { get; private set; }
        public Command<Stop> FavoriteCommand { get; set; } 
        public Command RandomCommand { get; set; }
        private string _filter;
        private ObservableRangeCollection<Stop> _filteredStops;

        public Command ChangeFavoriteCommand { get; set; }

        public StopsViewModel(Direction direction)
        {
            Direction = direction;
            _filteredStops = new ObservableRangeCollection<Stop>();
            _filteredStops.ReplaceRange(direction.Stops);

            Title = "Stops for  : " + Direction.Title;

            ChangeFavoriteCommand =  OptionsHelper.Instance.FavoriteCommand;
        }
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                OnPropertyChanged("FilteredStops");
            }
        }
        
        public ObservableRangeCollection<Stop> FilteredStops
        {
            get
            {
                if (string.IsNullOrEmpty(_filter))
                {
                    _filteredStops.ReplaceRange(Direction.Stops);
                }
                else
                {
                    var items = new List<Stop>(Direction.Stops.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));

                    _filteredStops.ReplaceRange(items);
                }
                return _filteredStops;
            }

        }
        

        
        

        


    }
}
