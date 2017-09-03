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
        StopsViewModel viewModel;
        public StopsPage()
        {
            InitializeComponent();
          

        }

        public StopsPage(Direction direction)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new StopsViewModel(direction);

            var map = new CustomMap
            {
                IsShowingUser = true,
                //HeightRequest = 300,

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,


            };

            foreach (var position in direction.Stops)
            {
                var pos = new Position(position.Lat, position.Lon);
                map.RouteCoordinates.Add(pos);
                //var pin = new Pin
                //{
                //    Type = PinType.Place,
                //    Position = pos,
                //    Label = position.Title,
                //    Address = position.Title,

                //};
                //map.Pins.Add(pin);
            }
            var firstPos = new Position(direction.Stops[0].Lat, direction.Stops[0].Lon);
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

        


    }
}