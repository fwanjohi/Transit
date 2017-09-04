using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopsPage : ContentPage
    {
        StopsViewModel _viewModel;
        public StopsPage()
        {
            InitializeComponent();
          

        }

        public StopsPage(Direction direction)
        {
            InitializeComponent();
            BindingContext = this._viewModel = new StopsViewModel(direction);

            var map = new CustomMap
            {
                IsShowingUser = true,
                //HeightRequest = 300,

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,


            };

            var cur = Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync().Result;
            var curPoint =  new Xamarin.Forms.Point(cur.Longitude, cur.Latitude);
            var curStop = new Stop { Lat = cur.Latitude, Lon = cur.Longitude };

            _viewModel.ClosestStop = null;
            Position firstPos = direction.Stops[0].Postion; 
            foreach (var stop in direction.Stops)
            {
                var pos = new Position(stop.Lat, stop.Lon);
                map.RouteCoordinates.Add(pos);

                var dist = curPoint.Distance (new Xamarin.Forms.Point(pos.Latitude, pos.Longitude));
                stop.Distance = TrackingHelper.ToMiles( TrackingHelper.CalculateDistance(curStop, stop));
                if (_viewModel.ClosestStop == null )
                {
                    _viewModel.ClosestStop = stop;
                }
                else
                {
                    if (_viewModel.ClosestStop.Distance > stop.Distance)
                    {
                        _viewModel.ClosestStop = stop;
                    }
                }

            }

            if (_viewModel.ClosestStop != null)
            {

                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = _viewModel.ClosestStop.Postion,
                    Label = _viewModel.ClosestStop.Title,
                    Address = _viewModel.ClosestStop.Display,

                };
                map.Pins.Add(pin);
                firstPos = _viewModel.ClosestStop.Postion;
            }
            
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(firstPos, Distance.FromMiles(0.5)));
            //map.MoveToRegion(MapSpan.FromCenterAndRadius(firstPos, Distance.FromMiles(0.5)));


            
            MapHolder.Children.Add(map);
        }

        private async void OnStopSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var stop = args.SelectedItem as Stop;
            if (stop == null)
                return;

            await Navigation.PushAsync(new PredictionsPage(stop));

            // Manually deselect item
            StopsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            

            //var dist = Plugin.Geolocator.CrossGeolocator.Current.

            //MyPosition = new Position(position.Latitude, position.Longitude);
        }

        private async Task BtnClosest_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PredictionsPage(_viewModel.ClosestStop));

            // Manually select select item
            StopsListView.SelectedItem = _viewModel.ClosestStop;
        }
    }
}