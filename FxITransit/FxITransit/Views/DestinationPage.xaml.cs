using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationPage : ContentPage
    {
        private DestinationViewModel _StopLiteViewModel;

        public DestinationPage(DestinationViewModel viewModel)
        {
            InitializeComponent();
            _StopLiteViewModel = viewModel;
            BindingContext = viewModel;
        }

       
        private async void StopListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {


            //var destination = e.Item as Destination;

            //if (destination != && destination.PosibleRoutes)
            //{
            //        // look for stops near you
            //        var stopsToDest = DbHelper.Instance.SearchStopsNearAddress(stop.Lat, stop.Lon, 0.3, stop.Title);

            //        if (stopsToDest.Count == 0)
            //        {
            //            var alertConfig = new AlertConfig
            //            {
            //                Message = $"No stops found near you to {stop.Title}. Please select another stop",
            //                OkText = "OK",
            //                OnAction = null
            //            };
            //            UserDialogs.Instance.Alert(alertConfig);

            //    }
            //    else
            //    {
            //        //go to predictions page
            //        await Navigation.PushAsync(new PredictionsPage(stop));
            //    }


            //}
        }
    }
}