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

        public bool ShowFavoritesOnly
        {
            get => Settings.Preference.FavoriteStopsOnly;
            set
            {
                Settings.Preference.FavoriteStopsOnly = value;
                Db.SavePrerefence(Settings.Preference);
                OnPropertyChanged("FilteredStops");
            }
        }

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
        public CustomMap Map
        {
            get => _map;
        }

        private string _filter;
        private ObservableRangeCollection<Stop> _filteredStops;
        public IViewContainer<View> TabsContainer { get; set; }
        public StopsViewModel(Route route)
        {


            _map = new CustomMap
            {
                MapType = MapType.Street,

            };

            _filteredStops = new ObservableRangeCollection<Stop>();

            Route = route;
            Directions = new ObservableCollection<Direction>();

            Title = "Stops for  : " + Route.Title;
            ChangeFavoriteCommand = PreferencesHelper.Instance.FavoriteCommand;
            DirectionChangedCommand = new Command<Direction>(ChangeDirection);
            if (Route.IsConfigured)
            {
                ChangeDirection(route.Directions.FirstOrDefault());
            }
        }

        public void ChangeDirection(Direction selecteDirection)
        {
            if (selecteDirection == null) return;
            selecteDirection.IsSeleted = true;
            SelectedDirection = selecteDirection;

            foreach (var direction in Directions)
            {
                if (direction != selecteDirection)
                {
                    direction.IsSeleted = false;
                }

            }

            var closest = TrackingHelper.Instance.GetClosestStop(SelectedDirection.Stops);
            ClosestStop = closest;
            if (closest != null)
            {
                var position = new Position(closest.Lat, closest.Lon); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = closest.Title,
                    Address = $"{closest.Distance} Miles"

                };
                _map.Pins.Add(pin);
                _map.MoveToRegion(MapSpan.FromCenterAndRadius(closest.Postion, Distance.FromMiles(0.5)));
            }



            foreach (var cnt in TabsContainer.Children)
            {
                var btn = cnt as Button;
                if (btn != null)
                {
                    var dir = cnt.BindingContext as Direction;
                    if (dir.IsSeleted)
                    {
                        btn.BackgroundColor = Color.FromHex("#2196F3");
                        btn.FontAttributes = FontAttributes.Bold;

                    }
                    else
                    {
                        btn.BackgroundColor = Color.LightGray;
                        btn.FontAttributes = FontAttributes.None;
                    }
                }
            }
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

            await TransitService.GetRouteDetails(Route);
            Directions.Clear();

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




            _map.MoveToRegion(MapSpan.FromCenterAndRadius(_map.RouteCoordinates[0], Distance.FromMiles(1.0)));
            SelectedDirection = Directions.FirstOrDefault();
            SelectedDirection.IsSeleted = true;

        }

    }
}
