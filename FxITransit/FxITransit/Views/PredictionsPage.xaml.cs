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
    public partial class PredictionsPage : ContentPage
    {
        PredictionsViewModel viewModel;
        public PredictionsPage()
        {
            InitializeComponent();
        }

        public PredictionsPage(Stop stop)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new PredictionsViewModel(stop);

            var map = new CustomMap
            {
                IsShowingUser = true,
                //HeightRequest = 300,

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                

            };

            var position = new Position(stop.Lat, stop.Lon); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = stop.Title,
                Address = stop.Title

            };

            foreach (var dirStop in stop.Direction.Stops)
            {
                map.RouteCoordinates.Add(new Position(dirStop.Lat, dirStop.Lon));
            }
            

            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));
            

            map.Pins.Add(pin);
            MapHolder.Children.Add(map);




        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (viewModel.Stop != null)
            {
                viewModel.LoadPredictionsCommand.Execute(null);
                viewModel.AutoRefreshPredictionsCommand.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (viewModel.Stop != null)
            {
                viewModel.AutoRefresh = false;
            }

        }
    }
}