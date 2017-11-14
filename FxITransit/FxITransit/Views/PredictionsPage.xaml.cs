using FxITransit.Models;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
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

            
            //Xamarin.Forms.Point.Distance

            //CLLocation pointA = new CLLocation(lat, long);
            //CLLocation pointB = new CLLocation(lat, long);
            //var distanceToB = pointB.DistanceFrom(pointA);
            Position position = new Position(0, 0);
            Pin pin = null;
            position = new Position(stop.Lat, stop.Lon); // Latitude, Longitude
            pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = stop.Title,
                Address = stop.Title

            };

            // var distance = Xamarin.Forms.Maps.Distance.()

            //foreach (var dirStop in stop.OtherStops)
            //{
                
            //    map.RouteCoordinates.Add(dirStop);
            //}

            //map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.5)));

            //map.Pins.Add(pin);
            //MapHolder.Children.Add(map);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (viewModel.Stop != null)
            {
                // make this the only stop to watch
                PreferencesHelper.Instance.OnPredictionsChanged = viewModel.OnPredictionsChanged;
                PreferencesHelper.Instance.ViewStopsToUpdate = new ObservableRangeCollection<Stop> {viewModel.Stop};
                PreferencesHelper.Instance.LoadPredictions();
                PreferencesHelper.Instance.StartAutoRefresh();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //since you could be in another view, clear what has to be watched 
            PreferencesHelper.Instance.ViewStopsToUpdate = new ObservableRangeCollection<Stop>();



        }

        
    }
}