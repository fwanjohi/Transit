using FxITransit.Models;
using FxITransit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopLitePage : ContentPage
    {
        private StopLiteViewModel _StopLiteViewModel;
        public StopLitePage(StopLiteViewModel viewModel)
        {
            InitializeComponent();
            _StopLiteViewModel = viewModel;
            BindingContext = viewModel;
        }

        private void BtnOK_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {

        }

        private async void StopListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var stop = e.Item as Stop;
            if (stop != null)
            {
                if (stop.IsDestinationStart)
                {
                    // look for stops near you
                    var stopsToDest = await DbHelper.Instance.SearchStopsNearMeToADestination(stop);

                    if (stopsToDest.Count == 0)
                    {
                        var alertConfig = new AlertConfig
                        {
                            Message = $"No stops found near you to {stop.Title}. Please select another stop",
                            OkText = "OK",
                            OnAction = null
                        };
                        UserDialogs.Instance.Alert(alertConfig);
                    }
                    else
                    {

                        var nearViwModel = new StopLiteViewModel(stopsToDest, true);
                        await Navigation.PushAsync(new StopLitePage(nearViwModel) { Title = $"Stops to {stop.Title}" });
                    }
                }
                else
                {
                    //go to predictions page
                    await Navigation.PushAsync(new PredictionsPage(stop));
                }


            }
        }
    }
}