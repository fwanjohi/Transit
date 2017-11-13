using FxITransit.Helpers;
using FxITransit.Models;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.ViewModels
{
    public class StopsViewModel : BaseViewModel
    {
        public Route Route { get; private set; }

        public ObservableCollection<Direction> Directions { get; set; }

        public Direction _selecteDirection;

        public Direction SelectedDirection
        {
            get { return _selecteDirection; }
            set
            {
                _selecteDirection = value;
                OnPropertyChanged("SelectedDirection");
                OnPropertyChanged("FilteredStops");
            }
        }
        public Command ChangeFavoriteCommand { get; set; }
        public Command<Direction> DirectionChangedCommand { get; set; }
        public CustomMap _map;

        private string _filter;
        private ObservableRangeCollection<Stop> _filteredStops;

        public StopsViewModel(Route route, CustomMap map)
        {
            _map = map;
            _filteredStops = new ObservableRangeCollection<Stop>();

            Route = route;
            Directions =new ObservableCollection<Direction>();

            Title = "Stops for  : " + Route.Title;
            ChangeFavoriteCommand = StopOptionsHelper.Instance.FavoriteCommand;
            DirectionChangedCommand = new Command<Direction>(OnDireChanged);
        }

        private void OnDireChanged(Direction selecteDirection)
        {
            SelectedDirection = selecteDirection;
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
                if (SelectedDirection != null)
                {
                    if (string.IsNullOrEmpty(_filter))
                    {
                        _filteredStops.ReplaceRange(SelectedDirection.Stops);
                    }
                    else
                    {
                        var items = new List<Stop>(SelectedDirection.Stops.Where(x => x.Title.ToLower().Contains(_filter.ToLower())));

                        _filteredStops.ReplaceRange(items);
                    }
                }
                return _filteredStops;
            }

        }

        public async Task ConfigureRoute()
        {
            if (!Route.IsConfigured)
            {
                await TransitService.GetRouteDetails(Route);
            }

            Position firstPos;
            foreach (var direction in Route.Directions)
            {
                Directions.Add(direction);

                firstPos = direction.Stops[0].Postion;
                foreach (var stop in direction.Stops)
                {
                    var mapPos = new Position(stop.Lat, stop.Lon);
                    _map.RouteCoordinates.Add(mapPos);
                }

            }
            SelectedDirection = Directions.FirstOrDefault();

        }
    }
}
