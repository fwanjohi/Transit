using FxITransit.Services.NextBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FxITransit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        private bool _first = true;
        public WelcomePage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
        }

        private void NextButton_OnClicked(object sender, EventArgs e)
        {

        }

        private void BtnRouteSetup_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgencyListView());
        }

        private void BtnFavourites_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FavouritesPage());
        }

        private void BtnSettings_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        private void BtnDest_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddressSearchPage());
        }

        protected override  void OnAppearing()
        {
            return;
            base.OnAppearing();
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                NextBusService.Instance.ConfigureInBackGround(cts.Token);
            }, cts.Token);

            _first = false;
        }


    }
}