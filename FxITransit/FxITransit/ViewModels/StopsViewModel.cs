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
        public StopsViewModel(Route route, Layouts.RepeatableStack stkDir)
        {

            TabsContainer = stkDir;
            _map = new CustomMap
            {
                MapType = MapType.Street,

            };

            _filteredStops = new ObservableRangeCollection<Stop>();

            Route = route;
            Directions = new ObservableCollection<Direction>(Route.Directions);
            OnPropertyChanged("Directions");

            Title = "Stops for  : " + Route.Title;
            ChangeFavoriteCommand = PreferencesHelper.Instance.FavoriteCommand;
            DirectionChangedCommand = new Command<Direction>(ChangeDirection);
            
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

            foreach (var cnt in TabsContainer.Children)
            {
                var btn = cnt as Button;
                if (btn != null)
                {
                    var dir = cnt.BindingContext as Direction;
                    if (dir.Equals(SelectedDirection))
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

            var closest = TrackingHelper.Instance.GetClosestStopToMe(SelectedDirection.Stops);
            ClosestStop = closest;

            Pin pin = Tracking.PinFromStop(closest); 


            var path = selecteDirection.Stops.Select(x => new GeoPoint(x));

            _map.DrawPath(path, new List<Pin> { pin }, closest, 1.0);

            if (PreferencesHelper.Instance.Preference.Speak && ClosestStop!= null)
            {
                var speak = $"The closest stop is {ClosestStop.DistanceAwayDisplay}";

                Utils.Speak(speak);
            }

            if (PreferencesHelper.Instance.Preference.Alert && ClosestStop != null)
            {
                var msg = $"The closest stop is {ClosestStop.DistanceAwayDisplay}";

                Utils.SendNotification(msg);
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

            _map.RouteCoordinates.Clear();

            Directions = Route.Directions;
            var closest = Tracking.GetClosestStopToMe(Route.Stops);

            var pin = Tracking.PinFromStop(closest);


            var path = Route.Stops.Select(x => new GeoPoint(x)).ToList();
            _map.DrawPath(path, pin);



        }



    }
}
