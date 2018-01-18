using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private async void OnSearchAddress(object sender, EventArgs e)
        {
            _viewModel.SearchAddress();
        }

        private void OnAddressSelected(object sender, ItemTappedEventArgs e)
        {
            
        }

        private async void StopListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var address = e.SelectedItem as GoogleAddress;
            if (address != null)
            {
                var  destination = await _viewModel.SearchDestinationRoutesTo(address);


                if (destination.PossibleRoutes.Count != 0)
                {
                    var viewModel = new DestinationViewModel(destination);
                    await Navigation.PushAsync(new DestinationPage(viewModel) { Title = $"Destinations to { address.Name } at {address.FormattedAddress}" });
                }
                else
                {
                    var alertConfig = new AlertConfig
                    {
                        Message = $"No stops found to {address.Name} from your location.\n Please select another address",
                        OkText = "OK",
                        OnAction = null
                    };
                    UserDialogs.Instance.Alert(alertConfig);
                }
            }
        }
    }
}