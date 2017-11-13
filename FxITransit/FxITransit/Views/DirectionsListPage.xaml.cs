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
    public partial class DirectionsListPage : ContentPage
    {
        DirectionsViewModel _viewModel;
        Route _route;
        private CustomMap _map;


        public DirectionsListPage()
        {
            InitializeComponent();
        }
        public DirectionsListPage(Route route)
        {
            InitializeComponent();
            _route = route;
            BindingContext = this._viewModel = new DirectionsViewModel(route);


            _map = new CustomMap
            {
                IsShowingUser = true,
                //HeightRequest = 300,

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,


            };
            //MapHolder.Children.Clear();

            MapHolder.Children.Add(_map);
            if (!_viewModel.Route.IsConfigured)
            {
                _viewModel.OnPopulateRoutesDone = OnRoutesPoulated;
                _viewModel.PopulateRouteCommand.Execute(null);

                Stop closest = null;
                foreach (var dir in _route.Directions)
                {
                    closest = TrackingHelper.Instance.GetClosestStop(dir.Stops);
                    if (closest != null)
                    {
                        var position = new Position(closest.Lat, closest.Lon); // Latitude, Longitude
                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = position,
                            Label = closest.Title,
                            Address = dir.Title

                        };
                        _map.Pins.Add(pin);
                    }
                }

                if (closest != null)
                {
                    _map.MoveToRegion(MapSpan.FromCenterAndRadius(closest.Postion, Distance.FromMiles(0.5)));
                }
            }

            


        }

        public void OnRoutesPoulated()
        {
            foreach(var dir in _viewModel.Route.Directions)
            {
                foreach (var dirStop in dir.Stops)
                {
                    _map.RouteCoordinates.Add(new Position(dirStop.Lat, dirStop.Lon));
                }

            }
        }

        

        async void OnDirectionSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //var direction = args.SelectedItem as Direction;
            //if (direction == null)
            //    return;

            //await Navigation.PushAsync(new StopsPage(direction));

            //// Manually deselect item
            //DirectionsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}