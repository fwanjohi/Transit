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
        private DestinationViewModel _viewModel;

        public DestinationPage(DestinationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
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

        private void ViewCell_BindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                var cell = sender as ViewCell;
                if (cell != null)
                {
                    var data = cell.BindingContext as PossibleRoute;
                    var index = _viewModel.Destination.PossibleRoutes.IndexOf(data);
                    cell.View.BackgroundColor = index % 2 == 0 ? Color.White : Color.LightBlue;

                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }

        }

    }
}