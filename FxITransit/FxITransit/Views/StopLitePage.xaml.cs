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
            var item = e.Item as StopLite;
            if (item != null)
            {
              
                var stoptToDest = await DbHelper.Instance.SearchStopsNearMeToADestination(item);
                var nearViwModel = new StopLiteViewModel(stoptToDest);
                if (item.IsStart)
                {
                    //go to predictions page
                    var stop = DbHelper.Instance.GetStopByTag(item.Tag);
                    await Navigation.PushAsync(new PredictionsPage(stop));

                }
                else
                {
                    Navigation.PushAsync(new StopLitePage(nearViwModel) {Title = $"Stops to {item.Title}"});
                }


            }
        }
    }
}