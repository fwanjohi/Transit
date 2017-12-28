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
                    var stoptToDest = await DbHelper.Instance.SearchStopsNearMeToADestination(stop);
                    var nearViwModel = new StopLiteViewModel(stoptToDest, true);
                    await Navigation.PushAsync(new StopLitePage(nearViwModel) { Title = $"Stops to {stop.Title}" });
                   
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