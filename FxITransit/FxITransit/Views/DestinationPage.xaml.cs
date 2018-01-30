using Acr.UserDialogs;
using FxITransit.Models;
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
            var route = e.Item as PossibleRoute;

            if (route != null)
            {
                await Navigation.PushAsync(new PredictionsPage(route.StartFrom));
            }
        }

    }
}