using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FxITransit.Models;
using FxITransit.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressSearchPage : ContentPage
    {
        private AddressSearchViewModel _viewModel;
        public AddressSearchPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AddressSearchViewModel();
        }

        private async void OnSearchFromAddress(object sender, EventArgs e)
        {
            SelectAddress(_viewModel.FromAddress, false);
        }

        private async void OnSearchToAddress(object sender, EventArgs e)
        {
              SelectAddress(_viewModel.ToAddress, true);

        }

        private async Task SelectAddress(string address, bool isDestination)
        {
            var found = await _viewModel.SearchAddress(address);

            if (found.Count != 0)
            {
                _viewModel.Addresses = new Helpers.ObservableRangeCollection<GoogleAddress>(found); ;
                await Navigation.PushModalAsync(new AddressFoundListPage (_viewModel, isDestination));
                _viewModel.UpdateProperties();
            }
        }

        private async void BtnSearchRoutes_Clicked(object sender, EventArgs e)
        {
            bool ok = true;
            string msg = "";
            if (_viewModel.SelectedFromAddress == null && !_viewModel.UseMyLocation)
            {
                ok = false;
                msg = "Please select a source address or select Use My Location\n";
            }

            if (_viewModel.SelectedToAddress == null)
            {
                ok = false;
                msg += "Please select a valid destination address";
            }

            if (ok)
            {
                var sourceAddress = _viewModel.UseMyLocation ? null : _viewModel.SelectedFromAddress;

                var destination = await _viewModel.SearchDestinationRoutesTo(sourceAddress, _viewModel.SelectedToAddress);


                if (destination.PossibleRoutes.Count != 0)
                {
                    var viewModel = new DestinationViewModel(destination);
                    await Navigation.PushAsync(new DestinationPage(viewModel) { Title = $"Destinations to { _viewModel.SelectedToAddress.FormattedAddress }" });
                }
                else
                {
                    var alertConfig = new AlertConfig
                    {
                        Message = $"No stops found to {_viewModel.SelectedToAddress.Name} from {_viewModel.SelectedToAddress.Name}. \n Please select another source address.",
                        OkText = "OK",
                        OnAction = null
                    };
                    UserDialogs.Instance.Alert(alertConfig);
                }
            }
            else
            {
                var alertConfig = new AlertConfig
                {
                    Message = msg,
                    OkText = "OK",
                    OnAction = null
                };
                UserDialogs.Instance.Alert(alertConfig);
            }
        }
    }
}