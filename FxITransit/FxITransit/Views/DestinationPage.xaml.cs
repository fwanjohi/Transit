using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Models;
using FxITransit.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DestinationPage : ContentPage
    {
        private DestinationViewModel _viewModel;
        public DestinationPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DestinationViewModel();
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
                var stopsFound =_viewModel.SearchStops(address).Result;

                if (stopsFound.Count !=0)
                {
                    var viewModel = new StopLiteViewModel(stopsFound, true);
                    await Navigation.PushAsync(new StopLitePage (viewModel) { Title = $"Stops near { address.Name } at {address.FormattedAddress}" });
                }
            }
        }
    }
}