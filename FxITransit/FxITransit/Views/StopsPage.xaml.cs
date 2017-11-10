using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services;
using FxITransit.ViewModels;
using Plugin.Geolocator;
using Plugin.TextToSpeech;
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
        CustomMap _map;
        public StopsPage()
        {
            InitializeComponent();
          

        }

        public StopsPage(Direction direction)
        {
            InitializeComponent();
            BindingContext = this._viewModel = new StopsViewModel(direction);

             _map = new CustomMap
            {
                IsShowingUser = true,
                //HeightRequest = 300,

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Position firstPos = direction.Stops[0].Postion;
            foreach (var stop in direction.Stops)
            {
                var mapPos = new Position(stop.Lat, stop.Lon);
                _map.RouteCoordinates.Add(mapPos);
            }
            
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(firstPos, Distance.FromMiles(0.5)));
            MapHolder.Children.Add(_map);

            
            var closest = TrackingHelper.Instance.GetClosestStop(_viewModel.Direction.Stops);
            _viewModel.ClosestStop = closest;
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
        }

        private async Task BtnClosest_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PredictionsPage(_viewModel.ClosestStop));
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }

        async void OnFavorite(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewItemPage());
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if ((sender as Button).BindingContext is Stop stop)
            {
                
                StopOptionsHelper.Instance.AddFavorite(stop);

            }
        }
    }
}